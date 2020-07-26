using System;
using Map.Client.Interfaces;
using Map.Client.Models;
using Newtonsoft.Json;
using Services.Core.Tools;

namespace Map.Client.Services
{
    public class LastLocationMessage : IMessage<LastLocation>
    {
        public LastLocation MessageObject { get; private set; }
        public bool CanParse(PlainMessage message)
        {
            if (message.Type != "LAST_LOCATION") return false;
            MessageObject = JsonConvert.DeserializeObject<LastLocation>(message.OBJ);
            return true;
        }

        public void PrintResult()
        {
            Console.WriteLine("{0} update @ {5:G}\n ◙ Location: {1}, {2} ◙ speed: {3:000}, ◙ Angel: {4:000}", 
                MessageObject.IMEI, 
                MessageObject.Location.Latitude, MessageObject.Location.Longitude, MessageObject.Location.Speed, MessageObject.Location.Angle, MessageObject.Location.Time);
        }
    }
}