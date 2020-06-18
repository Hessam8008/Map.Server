namespace Map.Client.Interfaces
{
    using Map.Client.Models;

    public interface IMessage<out T> 
        where T : IEntity
    {
        T MessageObject { get; }
        bool CanParse(PlainMessage message);
        void PrintResult();
    }
}