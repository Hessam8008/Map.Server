using System;

namespace GpsServer.Teltonika.Server.Args
{
    public class ErrorArgs
    {
        public ErrorArgs(string error)
        {
            Error = error;
        }

        public ErrorArgs(Exception ex)
        {
            Error = ex.Message;
        }

        public string Error { get; set; }
    }
}