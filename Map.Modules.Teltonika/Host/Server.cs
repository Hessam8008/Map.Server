// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="Server.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.Host
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Map.Models;
    using Map.Models.Args;

    /// <summary>
    /// Defines the <see cref="Server" />.
    /// </summary>
    public sealed class Server : IServer
    {
        #region Private varables
        /// <summary>
        /// The black box
        /// </summary>
        private readonly IBlackBox blackBox;

        /// <summary>
        /// The database settings
        /// </summary>
        private readonly IDatabaseSettings databaseSettings;

        /// <summary>
        /// Defines the listener.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The stopped
        /// </summary>
        private bool stopped;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Server" /> class.
        /// </summary>
        /// <param name="blackBox">The black box.</param>
        /// <param name="dataSettings">The database settings.</param>
        public Server(IBlackBox blackBox, IDatabaseSettings dataSettings)
        {
            this.blackBox = blackBox;
            this.databaseSettings = dataSettings;
        }

        #region Events definition
        /// <summary>
        /// The start server.
        /// </summary>
        public event OnServerStarted ServerStarted;

        /// <summary>
        /// The stop server.
        /// </summary>
        public event OnServerStopped ServerStopped;

        /// <summary>
        /// The connection accepted.
        /// </summary>
        public event OnConnectionAccepted ConnectionAccepted;

        /// <summary>
        /// The client authenticated.
        /// </summary>
        public event OnClientConnected ClientConnected;

        /// <summary>
        /// The client packet received.
        /// </summary>
        public event OnClientPacketReceived ClientPacketReceived;

        /// <summary>
        /// The client error.
        /// </summary>
        public event OnErrorOccured ErrorOccured;

        /// <summary>
        /// The log process activities.
        /// </summary>
        public event OnLogged Logged;

        /// <summary>
        /// The client disconnected.
        /// </summary>
        public event OnClientDisconnected ClientDisconnected;
        #endregion

        /// <summary>
        /// The Start.
        /// </summary>
        /// <param name="ip">IP for listening.</param>
        /// <param name="port">The port for listening.</param>
        /// <exception cref="Exception">Call Stop() before start.</exception>
        public void Start(string ip, int port)
        {
            if (this.listener != null)
            {
                throw new Exception("Call Stop() before start.");
            }

            var mainThread = new Task(async () => await this.Listening(ip, port));
            mainThread.Start();
        }

        /// <summary>
        /// The Stop.
        /// </summary>
        public void Stop()
        {
            this.stopped = true;
            this.listener?.Stop();
            this.listener = null;
            this.ServerStopped?.Invoke(this, new ServerStoppedArgs());
        }

        /// <summary>
        /// Listening the specified IP.
        /// </summary>
        /// <param name="ip">The IP.</param>
        /// <param name="port">The port.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private async Task Listening(string ip, int port)
        {
            this.listener = new TcpListener(IPAddress.Parse(ip), port);
            this.listener.Start();
            this.ServerStarted?.Invoke(this, new ServerStartedArgs(this.listener));

            while (!this.stopped)
            {
                TcpClient client;
                try
                {
                    client = await this.listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    this.ConnectionAccepted?.Invoke(this, new ConnectionAcceptedArgs(client));
                    var t = new Task(async () => { await this.HandleClient(client).ConfigureAwait(false); });
                    t.Start();
                }
                catch (Exception ex)
                {
                    if (!this.stopped)
                    {
                        this.ErrorOccured?.Invoke(this, new ErrorOccurredArgs(ex));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Handle device connected to server.
        /// </summary>
        /// <param name="tcpClient">The client<see cref="TcpClient" />.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private async Task HandleClient(TcpClient tcpClient)
        {
            var client = new Client(tcpClient, this.blackBox, this.databaseSettings);
            client.Connected += (sender, args) => this.ClientConnected?.Invoke(client, args);
            client.PacketReceived += (sender, args) => this.ClientPacketReceived?.Invoke(client, args);
            client.Disconnected += (sender, args) => this.ClientDisconnected?.Invoke(client, args);
            client.Logged += (sender, args) => this.Logged?.Invoke(client, args);
            client.ErrorOccurred += (sender, args) => this.ErrorOccured?.Invoke(this, args);
            try
            {
                await client.GetDataAsync();
            }
            catch (Exception e)
            {
                this.ErrorOccured?.Invoke(client, new ErrorOccurredArgs(e));
            }
        }
    }
}
