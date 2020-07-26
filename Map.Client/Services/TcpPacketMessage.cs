namespace Map.Client.Services
{
    using System;
    using System.Collections.Generic;
    using Map.Client.Interfaces;
    using Map.Client.Models;
    using Map.Models.AVL;
    using Newtonsoft.Json;


    public class TcpPacket : IEntity
    {
        public string IMEI { get; set; }

        public List<Location> Locations { get; set; }
    }


    public class TcpPacketMessage : IMessage<TcpPacket>
    {
        public TcpPacket MessageObject { get; private set; }

        public bool CanParse(PlainMessage message)
        {
            if (message.Type != "PACKET") 
                return false;
            try
            {
                this.MessageObject = JsonConvert.DeserializeObject<TcpPacket>(message.OBJ);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void PrintResult()
        {
            var temp_foreColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{this.MessageObject.IMEI}: {this.MessageObject.Locations.Count} location(s) received.");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var avl in this.MessageObject.Locations)
            {
                Console.WriteLine($"\t{avl.Time} | {avl.Latitude}, {avl.Longitude} | Speed: {avl.Speed} ");
            }

            Console.ForegroundColor = temp_foreColor;
        }
    }
}