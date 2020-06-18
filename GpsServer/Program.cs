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

namespace Map.Server
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Map.DataAccess;
    using Map.DataAccess.Gps;
    using Map.Modules.Teltonika;

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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task Main(string[] args)
        {
            const string Cs = "server=dm1server1;uid=dbUser;pwd=1234;database=GPS";

            try
            {
                MapUnitOfWork uow = new MapUnitOfWork(Cs);

                var device = await uow.DeviceRepository.GetByIMEI(358480085786194) ??
                             new Device
                             {
                                 IMEI = 358480085786194,
                                 Model = "TMT250",
                                 SN = "1102323094"
                             };

                device.Nickname = "حسام حسینی2";

                device = await uow.DeviceRepository.SyncAsync(device);
                uow.Commit();


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

                server.Start(3343);

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
    }
}
