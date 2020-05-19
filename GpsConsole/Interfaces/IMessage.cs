using GpsConsole.Models;

namespace GpsConsole.Interfaces
{
    public interface IMessage<out T> 
        where T : IEntity
    {
        T MessageObject { get; }
        bool CanParse(PlainMessage message);
        void PrintResult();
    }
}