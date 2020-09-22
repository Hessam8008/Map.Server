﻿// ***********************************************************************
// Assembly         : Map.Server
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="Program.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Server
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;

    using Map.DataAccess;
    using Map.Models;
    using Map.Models.Args;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The cache
        /// </summary>
        private static readonly ConcurrentDictionary<string, DeviceCache> Cache = new ConcurrentDictionary<string, DeviceCache>();
        
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The args<see cref="string" />.</param>
        public static void Main(string[] args)
        {
            // Main object
            ObjectiveMain();
        }

        /// <summary>
        /// Log the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        private static void Log(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0:HH:mm:ss}\n {1}\n\n", DateTime.Now, text);
            Console.ForegroundColor = tempColor;
        }

        /// <summary>
        /// Objectives the main.
        /// </summary>
        private static void ObjectiveMain()
        {
            try
            {
                var c = new DatabaseSettings();
                var bb = new TeltonikaBlackBox(c);

                IServer server = new Modules.Teltonika.Host.Server(bb, c);
                server.ServerStarted += (sender, e) => Log($"Server started on '{e.IP}:{e.Port}'.", ConsoleColor.Yellow);
                server.ServerStopped += (sender, e) => Log($"Server stopped.", ConsoleColor.Yellow);
                server.ConnectionAccepted += (sender, e) => Log($"Client accepted {e.RemoteIP}, port {e.Port}, Ttl {e.Ttl}", ConsoleColor.Green);
                server.ErrorOccured += (sender, e) => Log($"Error>\n{e.Exception}", ConsoleColor.Red);
                server.ClientDisconnected += (sender, e) => Log($"{e.IMEI} Disconnected.", ConsoleColor.Green);
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

        /// <summary>
        /// Servers the logged.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void Server_Logged(object sender, LoggedArgs e)
        {
            Log($"LOG: {e.Message}");
        }

        /// <summary>
        /// Clients the connected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static async void ClientConnected(object sender, ClientConnectedArgs e)
        {
            var message = $"{e.IMEI} Connected.";
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

        /// <summary>
        /// Clients the packet received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static async void ClientPacketReceived(object sender, ClientPacketReceivedArgs e)
        {
            try
            {
                Log(e.ToString(), ConsoleColor.Green);

                if (e.Locations.Count == 0)
                {
                    return;
                }

                Cache.TryGetValue(e.IMEI, out var dc);
                if (dc == null)
                {
                    dc = new DeviceCache();
                    MapUnitOfWork uow;
                    using (uow = new MapUnitOfWork(new DatabaseSettings()))
                    {
                        dc.Device = await uow.DeviceRepository.GetByIMEIAsync(e.IMEI);
                    }

                    Cache.TryAdd(e.IMEI, dc);
                }

                var last = e.Locations.OrderBy(a => a.Time).Last();
                if (dc.LastLocation == null)
                {
                    dc.LastLocation = last;
                    dc.LastStatus = last;
                    await NotifierService.BroadcastLastLocation(e.IMEI, last);
                }
                else if (last.Time >= dc.LastLocation.Time)
                {
                    if (last.Latitude > 0 && last.Longitude > 0)
                    {
                        dc.LastLocation = last;
                        dc.LastStatus = last;
                        await NotifierService.BroadcastLastLocation(e.IMEI, last);
                    }
                    else
                    {
                        dc.LastStatus = last;
                        await NotifierService.BroadcastLastStatus(e.IMEI, last);
                    }
                }

                await NotifierService.BroadcastPacket(e);
            }
            catch (Exception ex)
            {
                Log(ex.Message, ConsoleColor.Red);
            }
        }
    }
}