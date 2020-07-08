using System.Collections.Generic;
using System.Linq;

namespace Map.Modules.Teltonika.Models
{
    /// <summary>
    /// Defines the <see cref="TcpPacket" />.
    /// </summary>
    internal class TcpPacket
    {
        /// <summary>
        /// Gets or sets the HexMessage.
        /// </summary>
        public RawData RawMessage { get; set; }

        /// <summary>
        /// Gets or sets the Preamble.
        /// </summary>
        public int Preamble { get; set; }

        /// <summary>
        /// Gets or sets the DataFieldLength.
        /// </summary>
        public int DataFieldLength { get; set; }

        /// <summary>
        /// Gets or sets the Codec.
        /// </summary>
        public byte Codec { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfData1.
        /// </summary>
        public byte NumberOfData1 { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfData2.
        /// </summary>
        public byte NumberOfData2 { get; set; }

        /// <summary>
        /// Gets or sets the CRC16.
        /// </summary>
        public int CRC16 { get; set; }

        /// <summary>
        /// Gets or sets the Teltonika AVL Data.
        /// </summary>
        public List<Location> Locations { get; set; }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            var result = string.Empty;

            foreach (var location in this.Locations)
            {
                result += $"\n\tTime: {location.Timestamp}, Location: {location.Longitude}, {location.Latitude}";
            }

            return result;
        }

        public List<Map.Models.AVL.Location> ToAvlLocation()
        {
            return (from l in Locations select l.ToAvlLocation()).ToList();
        }

    }
}
