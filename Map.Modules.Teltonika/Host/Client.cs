// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="Client.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.Host
{
    using System;
    using System.Globalization;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using Map.Models;
    using Map.Models.Args;
    using Map.Modules.Teltonika.DataAccess;
    using Map.Modules.Teltonika.Host.Parsers;
    using Map.Modules.Teltonika.Models;

    /// <summary>
    /// The OnAuthenticated.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ClientConnectedArgs" />.</param>
    public delegate void OnConnected(object sender, ClientConnectedArgs e);

    /// <summary>
    /// The OnPacketReceived.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ClientPacketReceivedArgs" />.</param>
    public delegate void OnPacketReceived(object sender, ClientPacketReceivedArgs e);

    /// <summary>
    /// The OnDisconnected.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="EventArgs" />.</param>
    public delegate void OnDisconnected(object sender, DisconnectedArgs e);

    /// <summary>
    /// On error occurred.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void OnErrorOccurred(object sender, ErrorOccurredArgs e);


    /// <summary>
    /// Defines the <see cref="Client" />.
    /// </summary>
    internal class Client
    {
        /// <summary>
        /// The black box
        /// </summary>
        private readonly IBlackBox blackBox;

        /// <summary>
        /// The database settings
        /// </summary>
        private readonly IDatabaseSettings databaseSettings;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly TcpClient client;

        /// <summary>
        /// The IMEI of the device.
        /// </summary>
        private string imei;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client" /> class.
        /// </summary>
        /// <param name="client">The client<see cref="Client" />.</param>
        /// <param name="blackBox">The black box.</param>
        /// <param name="dataSettings">The database settings.</param>
        public Client(TcpClient client, IBlackBox blackBox, IDatabaseSettings dataSettings)
        {
            this.client = client;
            this.blackBox = blackBox;
            this.databaseSettings = dataSettings;
        }

        /// <summary>
        /// Defines the Authenticated.
        /// </summary>
        public event OnConnected Connected;

        /// <summary>
        /// Defines the PacketReceived.
        /// </summary>
        public event OnPacketReceived PacketReceived;

        /// <summary>
        /// Defines the Disconnected.
        /// </summary>
        public event OnDisconnected Disconnected;

        /// <summary>
        /// Occurs when something happened.
        /// </summary>
        public event OnLogged Logged;

        /// <summary>
        /// The error occurred.
        /// </summary>
        public event OnErrorOccurred ErrorOccurred;

        /// <summary>
        /// Get data from device.
        /// </summary>
        /// <returns>The <see cref="Task" />.</returns>
        /// <exception cref="Exception">Invalid IMEI format to open communication.</exception>
        /// <exception cref="Exception">Invalid IMEI length. IMEI must be 15.</exception>
        public async Task GetDataAsync()
        {
            try
            {
                var bytes = new byte[2048];
                var stream = this.client.GetStream();

                /*
                 * →│ PHASE 01 │←
                 */
                var counter = await stream.ReadAsync(bytes, 0, bytes.Length).ConfigureAwait(false);

                // Socket disconnected
                if (counter == 0)
                {
                    return;
                }

                /* First 2 bytes are IMEI length + next 15 bytes are IMEI  == 17 bytes
                    Sample: 000F333536333037303432343431303133 (HEX)
                 */
                if (counter != 17)
                {
                    throw new Exception("Invalid IMEI format to open communication.");
                }

                var hexImeiLen = BitConverter.ToString(bytes, 0, 2).Replace("-", string.Empty);
                var imeiLen = int.Parse(hexImeiLen, NumberStyles.HexNumber);

                /* Validate IMEI length has received by GPS */
                if (imeiLen != 15)
                {
                    throw new Exception($"Invalid IMEI length. IMEI must be 15 but it is {imeiLen}.");
                }

                /* Decode IMEI */
                this.imei = Encoding.ASCII.GetString(bytes, 2, 15);

                this.Connected?.Invoke(this, new ClientConnectedArgs(this.imei));

                if (!await this.blackBox.ApprovedIMEIAsync(this.imei).ConfigureAwait(false))
                {
                    await stream.WriteAsync(BitConverter.GetBytes(0)).ConfigureAwait(false);
                    this.Logged?.Invoke(this, new LoggedArgs($"IMEI {this.imei} Rejected."));
                    return;
                }

                this.Logged?.Invoke(this, new LoggedArgs($"IMEI {this.imei} Approved."));

                /* Reply acknowledge byte (01 = accept, 00 = reject) */
                await stream.WriteAsync(BitConverter.GetBytes(1)).ConfigureAwait(false);

                /*
                 *  →│ PHASE 02 │←
                 */
                var parser = new FmxParserCodec8();

                TeltonikaUnitOfWork uow;
                using (uow = new TeltonikaUnitOfWork(this.databaseSettings.ConnectionString))
                {
                    while ((counter = await stream.ReadAsync(bytes, 0, bytes.Length).ConfigureAwait(false)) != 0)
                    {
                        var hexData = BitConverter.ToString(bytes, 0, counter).Replace("-", string.Empty);
                        var rawMessage = new RawData(this.imei, hexData);

                        /* Save as log */
                        await uow.RawDataRepository.Insert(rawMessage).ConfigureAwait(false);
                        uow.Commit();
                        this.Logged?.Invoke(this, new LoggedArgs($"{this.imei}: Message saved in log table."));

                        /* Parse message */
                        var data = parser.Parse(rawMessage);
                        this.Logged?.Invoke(this, new LoggedArgs($"{this.imei}: Message parsed."));

                        /* Create a packet */
                        var packetArgs = new ClientPacketReceivedArgs(this.imei, data.ToAvlLocation());
                        this.Logged?.Invoke(
                            this,
                            new LoggedArgs($"{this.imei}: Packed created, locations > {data.Locations.Count}."));

                        /* Raise an event */
                        this.PacketReceived?.Invoke(this, packetArgs);

                        /* Check server action */
                        var acceptedLocation = await this.blackBox
                                                   .AcceptedLocationsAsync(this.imei, packetArgs.Locations)
                                                   .ConfigureAwait(false);
                        if (acceptedLocation)
                        {
                            this.Logged?.Invoke(this, new LoggedArgs($"{this.imei}: Locations accepted."));
                            await stream.WriteAsync(BitConverter.GetBytes((int)data.NumberOfData1))
                                .ConfigureAwait(false);
                        }
                        else
                        {
                            this.Logged?.Invoke(this, new LoggedArgs($"{this.imei}: Locations rejected."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorOccurred?.Invoke(this, new ErrorOccurredArgs(ex));
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// The CloseConnection.
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                this.client?.Close();
                this.client?.Dispose();
                this.Disconnected?.Invoke(this, new DisconnectedArgs(this.imei));
            }
            catch (Exception ex)
            {
                this.ErrorOccurred?.Invoke(this, new ErrorOccurredArgs(ex));
            }
        }
    }
}