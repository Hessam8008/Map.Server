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


namespace Map.Server
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
    public delegate void OnErrorOccured(object sender, ErrorOccuredArgs e);

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