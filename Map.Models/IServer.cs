// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="IServer.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models
{
    using System;

    using Map.Models.Args;

    /// <summary>
    /// The client connected.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ClientConnectedArgs" />.</param>
    public delegate void OnClientConnected(object sender, ClientConnectedArgs e);

    /// <summary>
    /// The packet received.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ClientPacketReceivedArgs" />.</param>
    public delegate void OnClientPacketReceived(object sender, ClientPacketReceivedArgs e);

    /// <summary>
    /// The OnError.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="ErrorOccurredArgs" />.</param>
    public delegate void OnErrorOccured(object sender, ErrorOccurredArgs e);

    /// <summary>
    /// Log every thing.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e<see cref="LoggedArgs" /></param>
    public delegate void OnLogged(object sender, LoggedArgs e);

    /// <summary>
    /// The OnDisconnected.
    /// </summary>
    /// <param name="sender">The sender<see cref="object" />.</param>
    /// <param name="e">The e<see cref="EventArgs" />.</param>
    public delegate void OnClientDisconnected(object sender, DisconnectedArgs e);

    /// <summary>
    /// On connection accepted.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e<see cref="ConnectionAcceptedArgs" />.</param>
    public delegate void OnConnectionAccepted(object sender, ConnectionAcceptedArgs e);

    /// <summary>
    /// On server started.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e<see cref="ServerStartedArgs" />.</param>
    public delegate void OnServerStarted(object sender, ServerStartedArgs e);

    /// <summary>
    /// On server stop.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e<see cref="ServerStoppedArgs" />.</param>
    public delegate void OnServerStopped(object sender, ServerStoppedArgs e);

    /// <summary>
    /// Interface IServer
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// The client authenticated.
        /// </summary>
        event OnClientConnected ClientConnected;

        /// <summary>
        /// The client packet received.
        /// </summary>
        event OnClientPacketReceived ClientPacketReceived;

        /// <summary>
        /// The client error.
        /// </summary>
        event OnErrorOccured ErrorOccured;

        /// <summary>
        /// The log process activities.
        /// </summary>
        event OnLogged Logged;

        /// <summary>
        /// The client disconnected.
        /// </summary>
        event OnClientDisconnected ClientDisconnected;

        /// <summary>
        /// The connection accepted.
        /// </summary>
        event OnConnectionAccepted ConnectionAccepted;

        /// <summary>
        /// The start server.
        /// </summary>
        event OnServerStarted ServerStarted;

        /// <summary>
        /// The stop server.
        /// </summary>
        event OnServerStopped ServerStopped;

        /// <summary>
        /// Start listening for clients.
        /// </summary>
        /// <param name="ip">IP for listening.</param>
        /// <param name="port">Port for listening.</param>
        void Start(string ip, int port);

        /// <summary>
        /// Stop listening.
        /// </summary>
        void Stop();
    }
}
