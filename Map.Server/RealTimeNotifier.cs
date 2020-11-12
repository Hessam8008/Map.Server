// ***********************************************************************
// Assembly         : Map.Server
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="NotifierService.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Server
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using Map.Models.AVL;
    using Map.Models.EventArgs;
    using Microsoft.AspNetCore.SignalR.Client;

    using Notifier.EndPoints.Service.Notification.Enums;
    using Notifier.EndPoints.Service.Notification.RequestsArg;

    using Services.Core.Interfaces;
    using Services.Core.Tools;
    using Services.WebApiCaller;
    using Services.WebApiCaller.Configuration;

    using UAC.EndPoints.Service.Base;

    /// <summary>
    /// Defines the real time notify.
    /// </summary>
    public static class RealTimeNotifier
    {
        /// <summary>
        /// The application token in notification service.
        /// </summary>
        private const string Token = "b3648cd2a9";

        /// <summary>
        /// The group name for hub.
        /// </summary>
        private const string GroupName = "Golriz.Gps";

        /// <summary>
        /// The hub connection.
        /// </summary>
        private static HubConnection connection;

        public static async Task BroadcastStartServer()
        {
            var message = new Dictionary<string, string>
                              {
                                  { "TYPE", "START_SERVER" },
                                  { "TIME", DateTime.UtcNow.ToString("O") },
                                  { "BODY", "Server has been started..." }
                              };

            await BroadcastMessage(message).ConfigureAwait(false);
        }

        /// <summary>
        /// The Broadcast IMEI.
        /// </summary>
        /// <param name="imei">
        /// The IMEI of the device.<see cref="string"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task BroadcastIMEI(string imei)
        {
            var message = new Dictionary<string, string>
                              {
                                  { "TYPE", "IMEI" },
                                  { "TIME", DateTime.UtcNow.ToString("O") },
                                  { "BODY", imei }
                              };

            await BroadcastMessage(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Broadcast packet asynchronously.
        /// </summary>
        /// <param name="arg">
        /// The argument.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task BroadcastPacket(ClientPacketReceivedArgs arg)
        {
            var message = new Dictionary<string, string>
                              {
                                  { "TYPE", "PACKET" },
                                  { "TIME", DateTime.UtcNow.ToString("O") },
                                  { "BODY", arg.ToJson() }
                              };
            await BroadcastMessage(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Broadcasts the last location.
        /// </summary>
        /// <param name="device">
        /// The IMEI.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task BroadcastLastLocation(Device device, Location location)
        {
            var message = new Dictionary<string, string>
                              {
                                  { "TYPE", "LAST_LOCATION" },
                                  { "TIME", DateTime.UtcNow.ToString("O") },
                                  { "BODY", new { Device = device, Location = location }.ToJson() }
                              };
            await BroadcastMessage(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Broadcasts the last status asynchronously.
        /// </summary>
        /// <param name="imei">
        /// The IMEI.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task BroadcastLastStatus(string imei, Location location)
        {
            var message = new Dictionary<string, string>
                              {
                                  { "TYPE", "STATUS" },
                                  { "TIME", DateTime.UtcNow.ToString("O") },
                                  { "BODY", new { IMEI = imei, Location = location }.ToJson() }
                              };
            await BroadcastMessage(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Cast objects to JSON format.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// The get connection hub.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task<HubConnection> getHub()
        {
            if (connection == null)
            {
                try
                {
                    IApiConfiguration apiConfig = new WinApiConfiguration();
                    apiConfig.Load();
                    var hubUrl = apiConfig.FindByTitle("Notifier.RealTimeHub");

                    if (hubUrl == null)
                    {
                        Console.WriteLine("Real time endpoint not found in configuration file.");
                        return null;
                    }

                    Console.WriteLine("Real time hub url: {0}", hubUrl.UrlAddress);

                    connection = new HubConnectionBuilder()
                        .WithUrl(new Uri(hubUrl.UrlAddress), options =>
                            {
                                options.Headers["AppToken"] = Token;
                            })
                        .WithAutomaticReconnect()
                        .Build();

                    //connection.On<Dictionary<string, string>>("NotifyAsync", ShowNotification);

                    ServicePointManager.DefaultConnectionLimit = 1;
                    await connection.StartAsync();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Create hub error:");
                    Console.WriteLine(e);
                }
            }

            return connection;
        }

        /// <summary>
        /// The broadcast message.
        /// </summary>
        /// <param name="message">The message of the notification.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private static async Task BroadcastMessage(Dictionary<string, string> message)
        {
            try
            {
                var hub = await getHub().ConfigureAwait(false);
                if (hub == null)
                {
                    Console.WriteLine("Cannot connect to real time hub for notification.");
                    return;
                }
                if (hub.State == HubConnectionState.Connected)
                {
                    await hub.InvokeAsync("NotifyAsync", GroupName, message).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("hub connection state: {0}", hub.State);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
