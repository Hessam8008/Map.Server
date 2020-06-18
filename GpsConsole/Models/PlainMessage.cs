namespace Map.Client.Models
{
    using System;

    public class PlainMessage
    {
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string OBJ { get; set; }
    }
}