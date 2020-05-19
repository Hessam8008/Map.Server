using GpsConsole.Models;

namespace GpsConsole.Interfaces
{
    public interface IMessageParser
    {
        IMessage<IEntity> Parse(PlainMessage message);
    }
}