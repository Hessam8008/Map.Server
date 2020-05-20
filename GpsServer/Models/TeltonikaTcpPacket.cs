namespace GpsServer.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="TeltonikaTcpPacket" />.
    /// </summary>
    public class TeltonikaTcpPacket
    {
        /// <summary>
        /// Gets or sets the IMEI.
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the HexMessage.
        /// </summary>
        public string HexMessage { get; set; }

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
        public List<TeltonikaAvlData> AvlData { get; set; }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            var result = string.Empty;

            foreach (var avl in this.AvlData)
            {
                result += $"\n  Time: {avl.Time}, Location: {avl.Longitude}, {avl.Latitude}\n";
            }

            return result;
        }
    }
}
