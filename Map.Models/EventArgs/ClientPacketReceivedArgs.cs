// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="ClientPacketReceivedArgs.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.EventArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Map.Models.AVL;

    /// <summary>
    /// Defines the <see cref="ClientPacketReceivedArgs" />.
    /// </summary>
    public class ClientPacketReceivedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPacketReceivedArgs" /> class.
        /// </summary>
        /// <param name="imei">IMEI of the device.</param>
        /// <param name="locations">List of locations.</param>
        public ClientPacketReceivedArgs(string imei, List<Location> locations)
        {
            this.IMEI = imei;
            this.Locations = locations;
        }

        /// <summary>
        /// Gets the IMEI.
        /// </summary>
        public string IMEI { get; }

        /// <summary>
        /// Gets the Packet.
        /// </summary>
        /// <value>The packet.</value>
        public List<Location> Locations { get; }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            var result = $"IMEI:{this.IMEI}";

            return this.Locations.Aggregate(result, (current, l) => current + $"\n{l.Time}| {l.Longitude} {l.Latitude} {l.Altitude} {l.Angle}, Speed={l.Speed}, Satellites={l.Satellites}");
        }
    }
}
