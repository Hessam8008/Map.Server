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


using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Map.Models;
using Map.Models.Args;

namespace Map.Modules.Teltonika.Host
{
    /// <summary>
    /// Defines the <see cref="Server" />.
    /// </summary>
    public sealed class Server : IServer
    {
        
        public event OnServerStarted ServerStarted;
        public event OnServerStopped ServerStopped;
        public event OnConnectionAccepted ConnectionAccepted;
        public event OnClientConnected ClientConnected;
        public event OnClientPacketReceived ClientPacketReceived;
        public event OnErrorOccured ErrorOccured;
        public event OnLogged Logged;
        public event OnClientDisconnected ClientDisconnected;
        
        /// <summary>
        /// Defines the listener.
        /// </summary>
        private TcpListener listener;

        private bool stopped = false;

        private readonly IBlackBox blackBox;
        private readonly IDatabaseSettings dbSettings;

        public Server(IBlackBox blackBox, IDatabaseSettings dbSettings)
        {
            this.blackBox = blackBox;
            this.dbSettings = dbSettings;
        }
        
        /// <summary>
        /// The Start.
        /// </summary>
        /// <param name="ip">IP for listening.</param>
        /// <param name="port"> The port for listening. </param>
        public void Start(string ip, int port)
        {
            if (listener != null)
                throw new Exception("Call Stop() before start.");

            var mainThread = new Task(async () => await Listening(ip, port));
            mainThread.Start();
        }

        private async Task Listening(string ip, int port)
        {
            this.listener = new TcpListener(IPAddress.Parse(ip), port);
            this.listener.Start();
            this.ServerStarted?.Invoke(this, new ServerStartedArgs(listener));

            while (!stopped)
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
                    if (!stopped)
                        ErrorOccured?.Invoke(this, new ErrorOccurredArgs(ex));
                    break;
                }
            }
        }

        /// <summary>
        /// The Stop.
        /// </summary>
        public void Stop()
        {
            stopped = true;
            this.listener?.Stop();
            listener = null;
            this.ServerStopped?.Invoke(this, new ServerStoppedArgs());
        }

        /// <summary>
        /// Handle device connected to server.
        /// </summary>
        /// <param name="tcpClient">The client<see cref="TcpClient" />.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private async Task HandleClient(TcpClient tcpClient)
        {
            var client = new Client(tcpClient, blackBox, dbSettings);
            client.Connected += (sender, args) => this.ClientConnected?.Invoke(client, args);
            client.PacketReceived += (sender, args) => this.ClientPacketReceived?.Invoke(client, args);
            client.Disconnected += (sender, args) => this.ClientDisconnected?.Invoke(client, args);
            client.Logged += (sender, args) => this.Logged?.Invoke(client, args);
            try
            {
                await client.GetDataAsync();
            }
            catch (Exception e)
            {
                ErrorOccured?.Invoke(client, new ErrorOccurredArgs(e));
            }
            
        }
    }
}
