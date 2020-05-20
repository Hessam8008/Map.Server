namespace GpsServer.Teltonika.Server.Args
{
    using GpsServer.Models;

    /// <summary>
    /// Defines the <see cref="PacketReceivedArgs" />.
    /// </summary>
    public class PacketReceivedArgs
    {
        /// <summary>
        /// Gets the Packet.
        /// </summary>
        public TeltonikaTcpPacket Packet { get; }

        /// <summary>
        /// Gets or sets a value indicating whether Accepted.
        /// </summary>
        public bool Accepted { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReceivedArgs"/> class.
        /// </summary>
        /// <param name="packet">The packet<see cref="TeltonikaTcpPacket"/>.</param>
        public PacketReceivedArgs(TeltonikaTcpPacket packet)
        {
            Packet = packet;
        }
    }
}
