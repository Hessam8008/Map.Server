namespace Map.Client.Models
{
    using System;
    using System.Collections.Generic;

    public class TeltonikaAvlData
    {
        public DateTime Time { get;  set; }
        public int Priority { get;  set; }
        public double Longitude { get;  set; }
        public double Latitude { get;  set; }
        public short Altitude { get;  set; }
        public short Angle { get;  set; }
        public byte Satellites { get;  set; }
        public short Speed { get;  set; }
        public byte  EventIOID { get; set; }
        public byte TotalIOElement { get; set; }
        public List<TeltonikaIoElement> IoElements { get; set; }
    }
}