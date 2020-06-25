namespace Map.Client.Models
{
    using System.Collections.Generic;

    using Map.Client.Interfaces;

    public class TeltonikaTcpPacket : IEntity
    {
        public TeltonikaTcpPacket()
        {
            
        }
        public string IMEI { get; set; }
        public string HexMessage { get; set; }
        public int Preamble { get; set; }
        public int DataFieldLength { get; set; }
        public byte Codec { get; set; }
        public byte NumberOfData1 { get; set; }
        public byte NumberOfData2 { get; set; }
        public int CRC16 { get; set; }
        public List<TeltonikaAvlData> AvlData { get; set; }

        public override string ToString()
        {
            var result = "";

            foreach (var avl in this.AvlData)
            {
                result += $"\n  Time: {avl.Time}, Location: {avl.Longitude}, {avl.Latitude}\n";
            }

            return result;
        }
    }
}