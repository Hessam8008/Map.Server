﻿namespace Map.Modules.Teltonika.Args
{
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// The start server args.
    /// </summary>
    public class ServerStartedArgs
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

    public class ConnectionAcceptedArgs
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
