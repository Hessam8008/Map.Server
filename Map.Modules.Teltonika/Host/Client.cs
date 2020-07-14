// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="Client.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Map.Models.Args;
using Map.Modules.Teltonika.DataAccess;
using Map.Modules.Teltonika.Host.Parsers;
using Map.Modules.Teltonika.Models;

namespace Map.Modules.Teltonika.Host
{
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
    /// The OnError.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ErrorArgs" />.</param>
    public delegate void OnError(object sender, ErrorArgs e);

    /// <summary>
    /// The OnDisconnected.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="EventArgs" />.</param>
    public delegate void OnDisconnected(object sender, DisconnectedArgs e);

    /// <summary>
    /// Defines the <see cref="Client" />.
    /// </summary>
    internal class Client
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly TcpClient client;

        /// <summary>
        /// The IMEI of the device.
        /// </summary>
        private string imei;
        
        private IBlackBox blackBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client" /> class.
        /// </summary>
        /// <param name="client">The client<see cref="Client" />.</param>
        public Client(TcpClient client, IBlackBox blackBox)
        {
            this.client = client;
            this.blackBox = blackBox;
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
        /// Defines the Error.
        /// </summary>
        public event OnError Error;

        /// <summary>
        /// Defines the Disconnected.
        /// </summary>
        public event OnDisconnected Disconnected;

        /// <summary>
        /// The GetData.
        /// </summary>
        /// <returns>The <see cref="Task" />.</returns>
        public async Task GetDataAsync()
        {
            try
            {
                var bytes = new byte[2048];
                var stream = this.client.GetStream();

                /*
                 * →│ PHASE 01 │←
                 */
                var counter = await stream.ReadAsync(bytes, 0, bytes.Length);//.ConfigureAwait(false);

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
                    this.Error?.Invoke(this, new ErrorArgs("Invalid IMEI format to open communication."));
                    return;
                }

                var hexImeiLen = BitConverter.ToString(bytes, 0, 2).Replace("-", string.Empty);
                var imeiLen = int.Parse(hexImeiLen, NumberStyles.HexNumber);

                /* Validate IMEI length has received by GPS */
                if (imeiLen != 15)
                {
                    this.Error?.Invoke(this, new ErrorArgs($"Invalid IMEI length. IMEI must be 15 but it is {imeiLen}."));
                    return;
                }

                /* Decode IMEI */
                this.imei = Encoding.ASCII.GetString(bytes, 2, 15);

                var connectedArg = new ClientConnectedArgs(this.imei);

                this.Connected?.Invoke(this, connectedArg);

                if (!await blackBox.ApprovedIMEIAsync(imei).ConfigureAwait(false))
                {
                    await stream.WriteAsync(BitConverter.GetBytes(0));//.ConfigureAwait(false);
                    Console.WriteLine("IMEI Rejected.");
                    return;
                }

                Console.WriteLine("IMEI Approved.");
                /* Reply acknowledge byte (01 = accept, 00 = reject) */
                await stream.WriteAsync(BitConverter.GetBytes(1));//.ConfigureAwait(false);

                /*
                 *  →│ PHASE 02 │←
                 */
                var parser = new FmxParserCodec8();
                using var uow = new TeltonikaUnitOfWork(blackBox.ConnectionString);
                while ((counter = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    var hexData = BitConverter.ToString(bytes, 0, counter).Replace("-", string.Empty);
                    var rawMessage = new RawData(imei, hexData);
                    await uow.RawDataRepository.Insert(rawMessage).ConfigureAwait(false);
                    uow.Commit();
                    Console.WriteLine("New message saved.");
                    var data = parser.Parse(rawMessage);
                    var packetArgs = new ClientPacketReceivedArgs(imei, data.ToAvlLocation());
                    this.PacketReceived?.Invoke(this, packetArgs);
                    var acceptedLocation = await blackBox.AcceptedLocationsAsync(imei, packetArgs.Locations).ConfigureAwait(false);
                    if (acceptedLocation)
                    {
                        Console.WriteLine("Accepted, NumberOfData1: {0}, NumberOfData2: {1}", (int)data.NumberOfData1, data.NumberOfData2);
                        await stream.WriteAsync(BitConverter.GetBytes((int)data.NumberOfData1));//.ConfigureAwait(false);
                    }
                    else
                    {
                        Console.WriteLine("Locations not accepted.");
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error?.Invoke(this, new ErrorArgs($"Client error. IMEI = {imei}", ex));
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
            catch (Exception e)
            {
                this.Error?.Invoke(this, new ErrorArgs($"Client close connection error. IMEI = {imei}", e));
            }
        }
    }
}