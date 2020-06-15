namespace GpsServer.Teltonika.Server
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using GpsServer.Teltonika.Client;

    /// <summary>
    /// Defines the <see cref="TeltonikaServer" />.
    /// </summary>
    public sealed class TeltonikaServer
    {
        /// <summary>
        /// Defines the listener.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The Start.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Start()
        {
            this.listener = new TcpListener(IPAddress.Any, 3343);
            this.listener.Start();
            Log($"Start listening on {this.listener.LocalEndpoint}", ConsoleColor.Magenta);
            do
            {
                Log("Waiting for a connection...", ConsoleColor.DarkCyan);
                TcpClient client;
                try
                {
                    client = await this.listener.AcceptTcpClientAsync();
                }
                catch
                {
                    break;
                }

                Log("Connected", ConsoleColor.Blue);
                var t = new Task(async () => { await this.HandleDevice(client); });
                t.Start();
            }
            while (true);
        }

        /// <summary>
        /// The Stop.
        /// </summary>
        public void Stop()
        {
            this.listener?.Stop();
        }

        /// <summary>
        /// The HandleDevice.
        /// </summary>
        /// <param name="client">The client<see cref="TcpClient"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task HandleDevice(TcpClient client)
        {
            var device = new TeltonikaDevice(client);
            device.Authenticated += async (sender, args) =>
            {
                Log($"Authentication OK. [IMEI: {args.IMEI}]", ConsoleColor.Yellow);
                args.Accepted = true;
                try
                {
                    await MessageCenter.BroadcastIMEI(args.IMEI).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log($"BroadcastIMEI Error: {ex.Message}", ConsoleColor.Red);
                }
            };

            device.Error += (sender, args) =>
            {
                Log($"Error: {args.Error}", ConsoleColor.Red);
            };

            device.PacketReceived += async (sender, args) =>
            {
                Log($"\nPacket [IMEI: {args.Packet.IMEI}, AVLs: {args.Packet.NumberOfData1}]: {args.Packet}", ConsoleColor.Green);
                args.Accepted = true;
                try
                {
                    await MessageCenter.BroadcastPacket(args.Packet).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log($"BroadcastPacket Error: {ex.Message}", ConsoleColor.Red);
                }
            };

            device.Disconnected += (sender, args) =>
            {
                Log("Disconnected", ConsoleColor.Blue);
            };

            await device.GetData();
        }

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="color">The color<see cref="ConsoleColor"/>.</param>
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
