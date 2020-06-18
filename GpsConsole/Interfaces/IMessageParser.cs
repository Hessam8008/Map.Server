namespace Map.Client.Interfaces
{
    using Map.Client.Models;

    public interface IMessageParser
    {
        IMessage<IEntity> Parse(PlainMessage message);
    }
}