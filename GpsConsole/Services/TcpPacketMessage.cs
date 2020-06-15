using System;
using GpsConsole.Interfaces;
using GpsConsole.Models;
using Newtonsoft.Json;

namespace GpsConsole.Services
{
    public class TcpPacketMessage : IMessage<TeltonikaTcpPacket>
    {
        public TeltonikaTcpPacket MessageObject { get; private set; }
        public bool CanParse(PlainMessage message)
        {
            if (message.Type != "PACKET") 
                return false;
            try
            {
                MessageObject = JsonConvert.DeserializeObject<TeltonikaTcpPacket>(message.OBJ);
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
            Console.WriteLine($"{MessageObject.IMEI}: {MessageObject.NumberOfData1} location(s) received.");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var avl in MessageObject.AvlData)
            {
                Console.WriteLine($"\t{avl.Time.ToLocalTime()} | {avl.Latitude}, {avl.Longitude} | Speed: {avl.Speed} ");
            }

            Console.ForegroundColor = temp_foreColor;
        }
    }
}