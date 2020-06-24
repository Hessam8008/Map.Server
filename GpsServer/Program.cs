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
    using System;

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
        public static void Main(string[] args)
        {
#if DEBUG
            const string Cs = "server=dm1server1;uid=dbUser;pwd=1234;database=GPS";
#else
            const string Cs = "server=10.10.1.12\\GCAS;database=GPSTrackerDB;uid=DVP1;pwd=Fly#3592;MultipleActiveResultSets=True;";
#endif
            Log(Cs);

            try
            {
                var server = new Server();
                server.ServerStarted += (sender, e) => Log($"Server started on {e.IP}, port {e.Port}", ConsoleColor.Yellow);
                server.ServerStopped += (sender, e) => Log($"Server stopped.", ConsoleColor.Yellow);
                server.ConnectionAccepted += (sender, e) => Log($"Client accepted {e.RemoteIP}, port {e.Port}, Ttl {e.Ttl}", ConsoleColor.Yellow);
                server.ClientError += (sender, e) => Log($"Error> IMEI={e.IMEI}, Error={e.Error}", ConsoleColor.Magenta);
                server.ClientAuthenticated += (sender, e) =>
                    {
                        var message = $"IMEI: {e.Imei}";
                        try
                        {
                            using MapUnitOfWork uow = new MapUnitOfWork(Cs);
                            var device = uow.DeviceRepository.GetByIMEI(e.Imei).ConfigureAwait(false).GetAwaiter().GetResult();
                            if (device == null)
                            {
                                message += ", new device";
                                device = new Device
                                             {
                                                 IMEI = e.Imei,
                                                 Model = "N/A",
                                                 MobileNumber = "0000000000",
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
                        }
                        catch (Exception exception)
                        {
                            message += $", error: {exception.Message}";
                            e.Accepted = false;
                        }
                        Log(message, ConsoleColor.Red);
                    };
                server.ClientPacketReceived += async (sender, e) =>
                    {
                        try
                        {
                            Log($"Packet [IMEI: {e.Packet.IMEI}, AVLs: {e.Packet.NumberOfData1}]: {e.Packet}",
                                ConsoleColor.Green);

                            using MapUnitOfWork db = new MapUnitOfWork(Cs);
                            var device = await db.DeviceRepository.GetByIMEI(e.Packet.IMEI);
                            if (device == null)
                            {
                                Log($"IMEI '{e.Packet.IMEI}' not found.", ConsoleColor.Red);
                                e.Accepted = false;
                                return;
                            }

                            var rawData = new RawData { IMEI = e.Packet.IMEI, PrimitiveMessage = e.Packet.HexMessage };

                            var rawDataId = await db.RawDataRepository.Insert(rawData);
                            db.Commit();

                            foreach (var location in e.Packet.Locations)
                            {
                                var loc = new Location(location)
                                              {
                                                  DeviceId = device.ID,
                                                  RawDataID = rawDataId,
                                                  Codec = e.Packet.Codec
                                              };
                                var locationId = await db.LocationRepository.Insert(loc);

                                foreach (var element in location.Elements)
                                {
                                    var el = new LocationElement(element)
                                                 {
                                                     LocationId = locationId
                                                 };
                                    await db.LocationElementRepository.Insert(el);
                                }
                            }
                            db.Commit();
                            e.Accepted = true;
                            Log("Data has been saved.", ConsoleColor.Green);
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message, ConsoleColor.Red);
                        }
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
