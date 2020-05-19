using System;

namespace GpsConsole.Models
{
    public class PlainMessage
    {
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string OBJ { get; set; }
    }
}