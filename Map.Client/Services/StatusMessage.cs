using System;
using Map.Client.Interfaces;
using Map.Client.Models;
using Newtonsoft.Json;
using Services.Core.Tools;

namespace Map.Client.Services
{
    public class StatusMessage : IMessage<StatusLocation>
    {
        public StatusLocation MessageObject { get; private set; }
        public bool CanParse(PlainMessage message)
        {
            if (message.Type != "LAST_STATUS") return false;
            MessageObject =  JsonConvert.DeserializeObject<StatusLocation>(message.OBJ);
            return true;
        }

        public void PrintResult()
        {
            Console.WriteLine("{0}: Status update @ {1:G}\n{2} ",
                MessageObject.IMEI,
                MessageObject.Location.Time, MessageObject.Location.Elements.ToJson());

        }
    }
}