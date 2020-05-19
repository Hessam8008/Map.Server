using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GpsServer.Teltonika.Client;
using GpsServer.Teltonika.Server;

namespace GpsServer
{
    class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                var server = new TeltonikaServer();
                var task = new Task(async () => { await server.Start(); });
                task.Start();

                Console.ReadKey();
                
                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }

    }
}
