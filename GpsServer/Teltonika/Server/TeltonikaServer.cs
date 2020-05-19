using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GpsServer.Teltonika.Client;

namespace GpsServer.Teltonika.Server
{
    using Notifier.Client.Libs.Windows.Services;

    public sealed class TeltonikaServer
    {
        Notifier.Client.Libs.Windows.Setting.ISettingProvider notifierSetting ;
        private INotifierService notifier;

        private TcpListener listener;
        public async Task Start()
        {
            localSetting = new LocalSetting();
            notifier = new NotifierService(this.localSetting);

            listener = new TcpListener(IPAddress.Any, 3343);
            listener.Start();
            Log($"Start listening on {listener.LocalEndpoint}", ConsoleColor.Magenta);
            do
            {
                Log("Waiting for a connection...", ConsoleColor.DarkCyan);
                TcpClient client;
                try
                {
                    client = await listener.AcceptTcpClientAsync();
                }
                catch (Exception)
                {
                    break;
                }

                Log("Connected", ConsoleColor.Blue);
                var t = new Task(async () =>
                {
                    await HandleDevice(client);
                });
                t.Start();


            } while (true);
        }

        public void Stop()
        {
            listener?.Stop();
        }

        private async Task HandleDevice(TcpClient client)
        {
            var device = new MyDevice(client);
            device.Authenticated += (sender, args) =>
            {
                Log($"Authentication OK. [IMEI: {args.IMEI}]", ConsoleColor.Yellow);
                args.Accepted = true;
                MessageCenter.BroadcastIMEI(args.IMEI);
            };

            device.Error += (sender, args) =>
            {
                Log($"Error: {args.Error}", ConsoleColor.Red);
            };

            device.PacketReceived += (sender, args) =>
            {
                Log($"\nPacket [IMEI: {args.Packet.IMEI}, AVLs: {args.Packet.NumberOfData1}]: {args.Packet}", ConsoleColor.Green);
                args.Accepted = true;
                MessageCenter.BroadcastPacket(args.Packet);
            };

            device.Disconnected += (sender, args) =>
            {
                Log("Disconnected", ConsoleColor.Blue);
            };

            await device.GetData();
        }

        private static void Log(string str, ConsoleColor color = ConsoleColor.Gray)
        {
            var stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(1)?.GetMethod()?.Name ?? "N/A";

            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0:HH:mm:ss} | {1}> {2}", DateTime.Now, methodName, str);
            Console.ForegroundColor = tempColor;
        }

    }
}
