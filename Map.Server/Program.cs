// ***********************************************************************
// Assembly         : GpsServer
// Author           : U12178
// Created          : 05-19-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="Program.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************



namespace Map.Server
{

    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Map.Models;
    using Map.Models.Args;
    using System;
    using System.Globalization;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">
        /// The args<see cref="string"/>.
        /// </param>
        public static void Main(string[] args)
        {
            ObjectiveMain();
        }

        /// <summary>
        /// Log the text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="color">
        /// The color.
        /// </param>
        private static void Log(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0:HH:mm:ss}\n {1}\n\n", DateTime.Now, text);
            Console.ForegroundColor = tempColor;
        }

        #region Simple

        public static void simpleMain()
        {
            try
            {
                new Thread(async () =>
                {
                    await Listening(IPAddress.Any.ToString(), 3343).ConfigureAwait(false);
                }).Start();

                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }



        private static async Task Listening(string ip, int port)
        {
            var listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();
            Log("Server started...");

            while (true)
            {
                try
                {
                    Log("Waiting....");
                    var client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    Log($"New client: {client.GetHashCode()}");
                    //var t = new Thread(HandleClient);
                    //t.Start(client);
                    var t = new Task(async () => { await HandleClient(client); });
                    t.Start();
                }
                catch (Exception ex)
                {
                    Log(ex.Message, ConsoleColor.Red);
                    break;
                }
            }
        }
        private static async Task HandleClient(object obj)
        {
            Log($"Handling {obj.GetHashCode()}...");
            var imei = "";
            var client = (TcpClient)obj;

            try
            {
                var bytes = new byte[2048];
                var stream = client.GetStream();

                /*
                 * →│ PHASE 01 │←
                 */
                var counter = await stream.ReadAsync(bytes, 0, bytes.Length);//.ConfigureAwait(false);

                // Socket disconnected
                if (counter == 0)
                {
                    return;
                }

                /* First 2 bytes are IMEI length + next 15 bytes are IMEI  == 17 bytes
                    Sample: 000F333536333037303432343431303133 (HEX)
                 */
                if (counter != 17)
                {
                    Log("Invalid IMEI format to open communication.");
                    return;
                }

                var hexImeiLen = BitConverter.ToString(bytes, 0, 2).Replace("-", string.Empty);
                var imeiLen = int.Parse(hexImeiLen, NumberStyles.HexNumber);

                /* Validate IMEI length has received by GPS */
                if (imeiLen != 15)
                {
                    Log($"Invalid IMEI length. IMEI must be 15 but it is {imeiLen}.");
                    return;
                }

                /* Decode IMEI */
                imei = Encoding.ASCII.GetString(bytes, 2, 15);

                var connectedArg = new ClientConnectedArgs(imei);

                Log($"Connected {imei}");

                /* Reply acknowledge byte (01 = accept, 00 = reject) */
                await stream.WriteAsync(BitConverter.GetBytes(1));

                await NotifierService.BroadcastIMEI(imei).ConfigureAwait(false);

                /*
                 *  →│ PHASE 02 │←
                 */
                var parser = new FmxParserCodec8();
                while ((counter = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    var hexData = BitConverter.ToString(bytes, 0, counter).Replace("-", string.Empty);
                    var data = parser.Parse(hexData);
                    Log($"Data received from {imei}: {data.NumberOfData2} records. First: {data.Locations.First().Timestamp} ");

                    await NotifierService.BroadcastPacket(new ClientPacketReceivedArgs(imei, data.ToAvlLocation()));

                    await stream.WriteAsync(BitConverter.GetBytes((int)data.NumberOfData1));
                }
            }
            catch (Exception ex)
            {
                Log($"Error: IMEI = {imei}, {ex.Message}");
            }
            finally
            {
                client?.Close();
                client?.Dispose();
                Log($"Disconnected {imei}.");
            }

        }

        #endregion

        #region Objective

        private static void ObjectiveMain()
        {

            try
            {
                var c = new TeltonikaConfiguration();
                var bb = new TeltonikaBlackBox(c);

                IServer server = new Modules.Teltonika.Host.Server(bb, c);
                server.ServerStarted += (sender, e) => Log($"Server started on '{e.IP}:{e.Port}'.", ConsoleColor.Yellow);
                server.ServerStopped += (sender, e) => Log($"Server stopped.", ConsoleColor.Yellow);
                server.ConnectionAccepted += (sender, e) => Log($"Client accepted {e.RemoteIP}, port {e.Port}, Ttl {e.Ttl}", ConsoleColor.Green);
                server.ErrorOccured += (sender, e) => Log($"Error>\n{e.Exception}", ConsoleColor.Red);
                server.ClientDisconnected += (sender, e) => Log($"Disconnected {e.IMEI}", ConsoleColor.Green);
                server.ClientConnected += ClientConnected;
                server.ClientPacketReceived += ClientPacketReceived;
                server.Logged += Server_Logged;

                server.Start(IPAddress.Any.ToString(), 3343);

                Console.WriteLine("Press any key to stop server...");
                Console.ReadKey();

                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Server stopped, press any key to close...");
            Console.ReadKey();
        }

        private static void Server_Logged(object sender, LoggedArgs e)
        {
            Log($"LOG: {e.Message}");
        }

        private static async void ClientConnected(object sender, ClientConnectedArgs e)
        {
            var message = $"{e.IMEI} connected.";
            try
            {
                await NotifierService.BroadcastIMEI(e.IMEI).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                message += $"\n * error: {exception.Message}";
            }

            Log(message, ConsoleColor.Green);
        }


        private static async void ClientPacketReceived(object sender, ClientPacketReceivedArgs e)
        {
            try
            {
                Log(e.ToString(), ConsoleColor.Green);
                await NotifierService.BroadcastPacket(e);
            }
            catch (Exception ex)
            {
                Log(ex.Message, ConsoleColor.Red);
            }

        }
        #endregion


    }
}