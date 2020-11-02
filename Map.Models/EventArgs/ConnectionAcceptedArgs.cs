// ***********************************************************************
// Assembly         : Map.Models
// Author           : Hessam Hosseini
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="ConnectionAcceptedArgs.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.EventArgs
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Class ConnectionAcceptedArgs.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class ConnectionAcceptedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionAcceptedArgs"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ConnectionAcceptedArgs(TcpClient client)
        {
            var remote = client.Client.RemoteEndPoint as IPEndPoint;
            this.RemoteIP = remote?.Address.ToString();
            this.Port = remote?.Port ?? 0;
            this.Ttl = client.Client.Ttl;
        }

        /// <summary>
        /// Gets the remote IP.
        /// </summary>
        /// <value>The remote IP.</value>
        public string RemoteIP { get; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; }

        /// <summary>
        /// Gets the TTL.
        /// </summary>
        /// <value>The TTL.</value>
        public short Ttl { get; }
    }
}
