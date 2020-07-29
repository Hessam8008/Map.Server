// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="ServerStartedArgs.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.Args
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// The start server args.
    /// </summary>
    public class ServerStartedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerStartedArgs"/> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public ServerStartedArgs(TcpListener listener)
        {
            var endPoint = listener.LocalEndpoint as IPEndPoint;
            this.IP = endPoint?.Address.ToString();
            this.Port = endPoint?.Port ?? 0;
        }

        /// <summary>
        /// Gets the IP.
        /// </summary>
        /// <value>The IP.</value>
        public string IP { get; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; }
    }
}