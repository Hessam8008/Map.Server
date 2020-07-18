using System;
using System.Net;
using System.Net.Sockets;

namespace Map.Models.Args
{
    /// <summary>
    /// The start server args.
    /// </summary>
    public class ServerStartedArgs: EventArgs
    {

        public string IP { get; }

        public int Port { get; }

        public ServerStartedArgs(TcpListener listener)
        {
            var endPoint = listener.LocalEndpoint as IPEndPoint;
            this.IP = endPoint?.Address.ToString();
            this.Port = endPoint?.Port ?? 0;
        }
    }
}