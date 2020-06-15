﻿// ***********************************************************************
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

namespace GpsServer
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using GPS.Modules.Teltonika;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The args<see cref="string" />.</param>
        public static void Main(string[] args)
        {
            try
            {
                var server = new Server();
                server.ServerStarted += (sender, e) => Log($"Server started on {e.IP}, port {e.Port}", ConsoleColor.Yellow);
                server.ServerStopped += (sender, e) => Log($"Server stopped.", ConsoleColor.Yellow);
                server.ConnectionAccepted += (sender, e) => Log($"Client accepted {e.RemoteIP}, port {e.Port}, Ttl {e.Ttl}", ConsoleColor.Yellow);
                server.ClientError += (sender, e) => Log($"Error> IMEI={e.IMEI}, Error={e.Error}", ConsoleColor.Magenta);
                server.ClientAuthenticated += (sender, e) =>
                    {
                        Log($"Authentication OK. [IMEI: {e.Imei}]", ConsoleColor.Red);
                        e.Accepted = true;
                    };
                server.ClientPacketReceived += (sender, e) =>
                    {
                        Log(
                                $"Packet [IMEI: {e.Packet.IMEI}, AVLs: {e.Packet.NumberOfData1}]: {e.Packet}",
                                ConsoleColor.Green);
                        e.Accepted = true;
                    };
                server.ClientDisconnected += (sender, e) =>
                    {
                        Log($"Disconnected {e.Imei}", ConsoleColor.Blue);
                    };

                var task = new Task(async () => { await server.Start(3343); });
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

        private static void Log(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0:HH:mm:ss}\n {1}\n\n", DateTime.Now, text);
            Console.ForegroundColor = tempColor;
        }

    }
}
