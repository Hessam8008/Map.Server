using System;

namespace Map.Models.Args
{
    public class ErrorOccuredArgs : EventArgs
    {
        public Exception Exception { get; }

        public DateTime Time { get; set; }

        public ErrorOccuredArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}