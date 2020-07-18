using System;

namespace Map.Models.Args
{
    public class LoggedArgs: EventArgs
    {
        public string Message { get; }

        public DateTime Time { get; }

        public LoggedArgs(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }
    }
}