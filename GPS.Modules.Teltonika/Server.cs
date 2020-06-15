// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="Server.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace GPS.Modules.Teltonika
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using GPS.Modules.Teltonika.Args;

    /// <summary>
    /// The OnAuthenticated.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="AuthenticatedArgs" />.</param>
    public delegate void OnClientAuthenticated(object sender, AuthenticatedArgs e);

    /// <summary>
    /// The OnPacketReceived.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="PacketReceivedArgs" />.</param>
    public delegate void OnClientPacketReceived(object sender, PacketReceivedArgs e);

    /// <summary>
    /// The OnError.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ErrorArgs" />.</param>
    public delegate void OnClientError(object sender, ErrorArgs e);

    /// <summary>
    /// The OnDisconnected.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="EventArgs" />.</param>
    public delegate void OnClientDisconnected(object sender, DisconnectedArgs e);

    /// <summary>
    /// On connection accepted.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void OnConnectionAccepted(object sender, ConnectionAcceptedArgs e);

    /// <summary>
    /// On server started.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void OnServerStarted(object sender, ServerStartedArgs e);

    /// <summary>
    /// On server stop.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void OnServerStopped(object sender, EventArgs e);


    /// <summary>
    /// Defines the <see cref="Server" />.
    /// </summary>
    public sealed class Server
    {
        /// <summary>
        /// Defines the listener.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The client authenticated.
        /// </summary>
        public event OnClientAuthenticated ClientAuthenticated;

        /// <summary>
        /// The client packet received.
        /// </summary>
        public event OnClientPacketReceived ClientPacketReceived;

        /// <summary>
        /// The client error.
        /// </summary>
        public event OnClientError ClientError;

        /// <summary>
        /// The client disconnected.
        /// </summary>
        public event OnClientDisconnected ClientDisconnected;

        /// <summary>
        /// The connection accepted.
        /// </summary>
        public event OnConnectionAccepted ConnectionAccepted;

        /// <summary>
        /// The start server.
        /// </summary>
        public event OnServerStarted ServerStarted;

        /// <summary>
        /// The stop server.
        /// </summary>
        public event OnServerStopped ServerStopped;

        /// <summary>
        /// The Start.
        /// </summary>
        /// <param name="port">The port for listening.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public async Task Start(int port)
        {
            this.listener = new TcpListener(IPAddress.Any, port);
            this.listener.Start();
            this.ServerStarted?.Invoke(this, new ServerStartedArgs(this.listener));
            do
            {
                TcpClient client;
                try
                {
                    client = await this.listener.AcceptTcpClientAsync();
                    this.ConnectionAccepted?.Invoke(this, new ConnectionAcceptedArgs(client));
                }
                catch
                {
                    break;
                }
                var t = new Task(async () => { await this.HandleClient(client); });
                t.Start();
            }
            while (true);
        }

        /// <summary>
        /// The Stop.
        /// </summary>
        public void Stop()
        {
            this.listener?.Stop();
            this.ServerStopped?.Invoke(this, new EventArgs());
            
        }

        /// <summary>
        /// The HandleDevice.
        /// </summary>
        /// <param name="client">The client<see cref="TcpClient" />.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private async Task HandleClient(TcpClient client)
        {
            var device = new Client(client);
            device.Authenticated += (sender, args) => this.ClientAuthenticated?.Invoke(device, args);
            device.Error += (sender, args) => this.ClientError?.Invoke(device, args);
            device.PacketReceived += (sender, args) => this.ClientPacketReceived?.Invoke(device, args);
            device.Disconnected += (sender, args) => this.ClientDisconnected?.Invoke(device, args);
            
            await device.GetData();
        }
    }
}
