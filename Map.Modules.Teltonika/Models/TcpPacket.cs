// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="TcpPacket.cs" company="Map.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="TcpPacket" />.
    /// </summary>
    internal class TcpPacket
    {
        /// <summary>
        /// Gets or sets the HexMessage.
        /// </summary>
        /// <value>The raw message.</value>
        public RawData RawMessage { get; set; }

        /// <summary>
        /// Gets or sets the Preamble.
        /// </summary>
        /// <value>The preamble.</value>
        public int Preamble { get; set; }

        /// <summary>
        /// Gets or sets the DataFieldLength.
        /// </summary>
        /// <value>The length of the data field.</value>
        public int DataFieldLength { get; set; }

        /// <summary>
        /// Gets or sets the Codec.
        /// </summary>
        /// <value>The codec.</value>
        public byte Codec { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfData1.
        /// </summary>
        /// <value>The number of data1.</value>
        public byte NumberOfData1 { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfData2.
        /// </summary>
        /// <value>The number of data2.</value>
        public byte NumberOfData2 { get; set; }

        /// <summary>
        /// Gets or sets the CRC16.
        /// </summary>
        /// <value>The cr C16.</value>
        public int CRC16 { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        /// <value>The list of location.</value>
        public List<Location> Locations { get; set; }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return this.Locations.Aggregate(string.Empty, (current, location) => current + $"\n\tTime: {location.Timestamp}, Location: {location.Longitude}, {location.Latitude}");
        }

        /// <summary>
        /// Converts to AVL location.
        /// </summary>
        /// <returns>returns list of <exception cref="Map.Models.AVL.Location"></exception>.</returns>
        public List<Map.Models.AVL.Location> ToAvlLocation()
        {
            return (from l in this.Locations select l.ToAvlLocation()).ToList();
        }
    }
}
