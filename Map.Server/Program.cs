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


using System.Net;
using System.Net.Http.Headers;
using Map.Models.Args;
using Map.Models.AVL;

namespace Map.Server
{
    using Map.DataAccess;
    using System;

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

            var bb = new TeltonikaBlackBox();
            Log(bb.ConnectionString);

            try
            {
                Models.IServer server = new Modules.Teltonika.Host.Server(bb);
                server.ServerStarted += (sender, e) => Log($"Server started on {e.IP}, port {e.Port}", ConsoleColor.Yellow);
                server.ServerStopped += (sender, e) => Log($"Server stopped.", ConsoleColor.Yellow);
                server.ConnectionAccepted += (sender, e) => Log($"Client accepted {e.RemoteIP}, port {e.Port}, Ttl {e.Ttl}", ConsoleColor.Yellow);
                server.Error += (sender, e) => Log($"Error> {e.Message}\n{e.Exception}", ConsoleColor.Magenta);
                server.ClientDisconnected += (sender, e) => Log($"Disconnected {e.IMEI}", ConsoleColor.Blue);
                server.ClientConnected += ClientConnected;
                server.ClientPacketReceived += ClientPacketReceived;


                server.Start(IPAddress.Any.ToString(), 3343);

                Console.WriteLine("Press any key to close...");
                Console.ReadKey();

                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
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

        private static void ClientConnected(object sender, ClientConnectedArgs e)
        {
            var message = $"{e.IMEI} connected.";
            try
            {
                NotifierService.BroadcastIMEI(e.IMEI).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                message += $"\n * error: {exception.Message}";
            }
            Log(message, ConsoleColor.Red);
        }


        private static void ClientPacketReceived(object sender, ClientPacketReceivedArgs e)
        {
            try
            {
                Log(e.ToString(), ConsoleColor.Green);
                NotifierService.BroadcastPacket(e).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log(ex.Message, ConsoleColor.Red);
            }

        }


    }
}