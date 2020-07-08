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
#if DEBUG
        const string Cs = "server=dm1server1;uid=dbUser;pwd=1234;database=GPS;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#else
        const string Cs = "server=10.10.1.12\\GCAS;database=GPSTrackerDB;uid=DVP1;pwd=Fly#3592;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#endif

        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">
        /// The args<see cref="string"/>.
        /// </param>
        public static void Main(string[] args)
        {

            Log(Cs);
            var config = new TeltonikaConfig(Cs);
            try
            {
                Models.IServer server = new Modules.Teltonika.Host.Server(config);
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
            var message = $"IMEI: {e.IMEI}";
            try
            {
                using var uow = new MapUnitOfWork(Cs);
                var device = uow.DeviceRepository.GetByIMEIAsync(e.IMEI).ConfigureAwait(false).GetAwaiter().GetResult();
                if (device == null)
                {
                    message += ", new device";
                    device = new Device
                    {
                        IMEI = e.IMEI,
                        Model = "N/A",
                        SimNumber = "0000000000",
                        OwnerMobileNumber = "0000000000",
                        Nickname = "N/A",
                        SN = "N/A"
                    };
                    device = uow.DeviceRepository.SyncAsync(device).ConfigureAwait(false).GetAwaiter().GetResult();
                    uow.Commit();
                    message += $"\n{device.ToJson()}";
                }
                else
                {
                    message += ", exists device";
                }
                e.Accepted = true;
                //NotifierService.BroadcastIMEI(e.IMEI).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                message += $", error: {exception.Message}";
                e.Accepted = false;
            }
            Log(message, ConsoleColor.Red);
        }


        private static async void ClientPacketReceived(object sender, ClientPacketReceivedArgs e)
        {
            try
            {
                Log(e.ToString(), ConsoleColor.Green);

                using var db = new MapUnitOfWork(Cs);
                var device = await db.DeviceRepository.GetByIMEIAsync(e.IMEI).ConfigureAwait(false);
                if (device == null)
                {
                    Log($"IMEI '{e.IMEI}' not found.", ConsoleColor.Red);
                    e.Accepted = false;
                    return;
                }
                foreach (var location in e.Locations)
                {
                    location.Device = device;
                    var locationId = await db.LocationRepository.Insert(location).ConfigureAwait(false);
                }
                db.Commit();
                e.Accepted = true;
                Log("Data has been saved.", ConsoleColor.Green);
                //await NotifierService.BroadcastPacket(e);
            }
            catch (Exception ex)
            {
                Log(ex.Message, ConsoleColor.Red);
            }

        }


    }
}