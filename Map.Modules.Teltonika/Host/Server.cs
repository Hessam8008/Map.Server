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

        public event OnConnectionAccepted ConnectionAccepted;
        public event OnClientConnected ClientConnected;
        public event OnClientPacketReceived ClientPacketReceived;
        public event OnClientDisconnected ClientDisconnected;


        public event Map.Models.OnError Error;
        public event OnServerStarted ServerStarted;
        public event OnServerStopped ServerStopped;


        /// <summary>
        /// Defines the listener.
        /// </summary>
        private TcpListener listener;

        private bool stopped = false;

        private IBlackBox blackBox;
        public Server(IBlackBox blackBox)
        {
            this.blackBox = blackBox;
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
                        Error?.Invoke(this, new ErrorArgs("Server error.", ex));
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
        /// <param name="client">The client<see cref="TcpClient" />.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private async Task HandleClient(TcpClient client)
        {
            var device = new Client(client, blackBox);
            device.Connected += (sender, args) => this.ClientConnected?.Invoke(device, args);
            device.Error += (sender, args) => this.Error?.Invoke(device, args);
            device.PacketReceived += (sender, args) => this.ClientPacketReceived?.Invoke(device, args);
            device.Disconnected += (sender, args) => this.ClientDisconnected?.Invoke(device, args);

            await device.GetDataAsync();
        }
    }
}
