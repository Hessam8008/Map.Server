using GpsServer.Models;

namespace GpsServer.Teltonika.Server.Args
{
    public class PacketReceivedArgs
    {
        public TeltonikaTcpPacket Packet { get; }

        public bool Accepted { get; set; } = false;

        public PacketReceivedArgs(TeltonikaTcpPacket packet)
        {
            Packet = packet;
        }
        
    }
}