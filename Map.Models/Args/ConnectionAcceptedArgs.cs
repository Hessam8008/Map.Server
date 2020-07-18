using System;
using System.Net;
using System.Net.Sockets;

namespace Map.Models.Args
{
    public class ConnectionAcceptedArgs: EventArgs
    {
        public string RemoteIP { get; }

        public int Port { get; }

        public short Ttl { get; }

        public ConnectionAcceptedArgs(TcpClient client)
        {
            var remote = client.Client.RemoteEndPoint as IPEndPoint;
            this.RemoteIP = remote?.Address.ToString();
            this.Port = remote?.Port ?? 0;
            this.Ttl = client.Client.Ttl;
        }
    }
}
