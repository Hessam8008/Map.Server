namespace Notifier.Client
{
    public class MessageReceivedArgs
    {
        public MessageReceivedArgs(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}