using System;
using System.Collections.Generic;
using System.Text;
using Map.Models.Args;

namespace Map.Models
{
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
    /// <param name="e">The e<see cref="ErrorArgs" />.</param>
    public delegate void OnErrorOccured(object sender, ErrorOccuredArgs e);
    
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
    /// <param name="sender"> The sender. </param>
    /// <param name="e"> The e. </param>
    public delegate void OnConnectionAccepted(object sender, ConnectionAcceptedArgs e);

    /// <summary>
    /// On server started.
    /// </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e"> The e. </param>
    public delegate void OnServerStarted(object sender, ServerStartedArgs e);

    /// <summary>
    /// On server stop.
    /// </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e"> The e. </param>
    public delegate void OnServerStopped(object sender, ServerStoppedArgs e);


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

        void Start(string ip, int port);

        void Stop();

    }
}
