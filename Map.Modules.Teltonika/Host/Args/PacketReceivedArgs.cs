// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="PacketReceivedArgs.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Map.Models.AVL;

namespace Map.Modules.Teltonika.Args
{
    /// <summary>
    /// Defines the <see cref="PacketReceivedArgs" />.
    /// </summary>
    public class PacketReceivedArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReceivedArgs" /> class.
        /// </summary>
        /// <param name="packet">The packet<see cref="TcpPacket" />.</param>
        public PacketReceivedArgs(TcpPacket packet)
        {
            this.Packet = packet;
        }

        /// <summary>
        /// Gets the Packet.
        /// </summary>
        /// <value>The packet.</value>
        public TcpPacket Packet { get; }

        /// <summary>
        /// Gets or sets a value indicating whether Accepted.
        /// </summary>
        /// <value><c>true</c> if accepted; otherwise, <c>false</c>.</value>
        public bool Accepted { get; set; } = false;
    }
}
