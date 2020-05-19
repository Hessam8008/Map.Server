using System.Collections.Generic;
using GpsConsole.Interfaces;
using GpsConsole.Models;

namespace GpsConsole.Services
{
    public class MessageParser2 : IMessageParser
    {
        public IMessage<IEntity> Parse(PlainMessage message)
        {

            List<IMessage<IEntity>> list = new List<IMessage<IEntity>>
            {
                new IMEIMessage(), new TcpPacketMessage()
            };

            var result = list.Find(x=>x.CanParse(message));
            return result;
        }
    }
}