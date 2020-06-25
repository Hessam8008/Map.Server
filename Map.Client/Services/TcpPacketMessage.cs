namespace Map.Client.Services
{
    using System;

    using Map.Client.Interfaces;
    using Map.Client.Models;

    using Newtonsoft.Json;

    public class TcpPacketMessage : IMessage<Map.Models.AVL.Location>
    {
        public TeltonikaTcpPacket MessageObject { get; private set; }
        public bool CanParse(PlainMessage message)
        {
            if (message.Type != "PACKET") 
                return false;
            try
            {
                this.MessageObject = JsonConvert.DeserializeObject<TeltonikaTcpPacket>(message.OBJ);
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
            Console.WriteLine($"{this.MessageObject.IMEI}: {this.MessageObject.NumberOfData1} location(s) received.");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var avl in this.MessageObject.AvlData)
            {
                Console.WriteLine($"\t{avl.Time.ToLocalTime()} | {avl.Latitude}, {avl.Longitude} | Speed: {avl.Speed} ");
            }

            Console.ForegroundColor = temp_foreColor;
        }
    }
}