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

using System.Collections.Generic;
using Map.Models.AVL;

namespace Map.Models.Args
{
    /// <summary>
    /// Defines the <see cref="ClientPacketReceivedArgs" />.
    /// </summary>
    public class ClientPacketReceivedArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketReceivedArgs" /> class.
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="locations"></param>
        public ClientPacketReceivedArgs(string imei, List<Location> locations)
        {
            IMEI = imei;
            Locations = locations;
        }

        public string IMEI { get; }

        /// <summary>
        /// Gets the Packet.
        /// </summary>
        /// <value>The packet.</value>
        public List<Location> Locations { get; }

        /// <summary>
        /// Gets or sets a value indicating whether Accepted.
        /// </summary>
        /// <value><c>true</c> if accepted; otherwise, <c>false</c>.</value>
        public bool Accepted { get; set; } = false;


        public override string ToString()
        {
            var result = $"IMEI:{IMEI}";

            foreach (var l in Locations)
            {
                result +=
                    $"\n{l.Time}| {l.Longitude} {l.Latitude} {l.Altitude} {l.Angle}, Speed={l.Speed}, Satellites={l.Satellites}";
            }
            return result;
        }
    }
}
