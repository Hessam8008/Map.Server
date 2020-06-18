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
namespace Map.Modules.Teltonika
{
    using System;
    using System.Globalization;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using Map.Modules.Teltonika.Args;

    /// <summary>
    /// The OnAuthenticated.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="AuthenticatedArgs" />.</param>
    public delegate void OnAuthenticated(object sender, AuthenticatedArgs e);

    /// <summary>
    /// The OnPacketReceived.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="PacketReceivedArgs" />.</param>
    public delegate void OnPacketReceived(object sender, PacketReceivedArgs e);

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
    public class Client
    {
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
        public Client(TcpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Defines the Authenticated.
        /// </summary>
        public event OnAuthenticated Authenticated;

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
        public async Task GetData()
        {
            try
            {
                var bytes = new byte[2048];
                var stream = this.client.GetStream();

                /*
                 * --► PHASE 01 ◄--
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
                    this.Error?.Invoke(this, new ErrorArgs("Authentication failed.", "N/A"));
                    return;
                }

                var hexImeiLen = BitConverter.ToString(bytes, 0, 2).Replace("-", string.Empty);
                var imeiLen = int.Parse(hexImeiLen, NumberStyles.HexNumber);

                /* Validate IMEI length has received by GPS */
                if (imeiLen != 15)
                {
                    this.Error?.Invoke(this, new ErrorArgs($"Invalid IMEI length. IMEI must be 15 but it is {imeiLen}.", "N/A"));
                    return;
                }

                /* Decode IMEI */
                this.imei = Encoding.ASCII.GetString(bytes, 2, 15);

                var connectedArg = new AuthenticatedArgs(this.imei);
                this.Authenticated?.Invoke(this, connectedArg);

                if (!connectedArg.Accepted)
                {
                    await stream.WriteAsync(BitConverter.GetBytes(0)).ConfigureAwait(false);
                    return;
                }

                /* Reply acknowledge byte (01 for accept, 00 for reject) */
                await stream.WriteAsync(BitConverter.GetBytes(1)).ConfigureAwait(false);

                /*
                 * --► PHASE 02 ◄--
                 */
                while ((counter = await stream.ReadAsync(bytes, 0, bytes.Length).ConfigureAwait(false)) != 0)
                {
                    var hexData = BitConverter.ToString(bytes, 0, counter).Replace("-", string.Empty);
                    var parser = new FmxParserCodec8();
                    var data = parser.Parse(hexData);
                    data.IMEI = this.imei;
                    var packetArgs = new PacketReceivedArgs(data);
                    this.PacketReceived?.Invoke(this, packetArgs);
                    if (packetArgs.Accepted)
                    {
                        await stream.WriteAsync(BitConverter.GetBytes((int)data.NumberOfData1)).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error?.Invoke(this, new ErrorArgs(ex, this.imei));
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
                Console.WriteLine(e);
            }
        }
    }
}