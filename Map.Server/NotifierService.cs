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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Map.Models.AVL;
    using Map.Models.EventArgs;

    using Notifier.EndPoints.Service.Base;
    using Notifier.EndPoints.Service.Notification.Enums;
    using Notifier.EndPoints.Service.Notification.RequestsArg;

    using Services.Core.Interfaces;
    using Services.WebApiCaller;
    using Services.WebApiCaller.Configuration;

    using UAC.EndPoints.Service.Base;

    /// <summary>
    /// Defines the <see cref="NotifierService" />.
    /// </summary>
    public static class NotifierService
    {
        /// <summary>
        /// The notifier service
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static INotifierService notifierService;

        /// <summary>
        /// The API caller
        /// </summary>
        private static IApiCaller apiCaller;

        /// <summary>
        /// The API configuration
        /// </summary>
        private static IApiConfiguration apiConfiguration;

        /// <summary>
        /// The UAC service.
        /// </summary>
        private static UserAccountService uac;

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
            var obj = new
            {
                TYPE = "IMEI",
                TIME = DateTime.UtcNow,
                OBJ = imei
            };
            await BroadcastMessage(obj.ToJson()).ConfigureAwait(false);
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
            var obj = new
            {
                TYPE = "PACKET",
                TIME = DateTime.UtcNow,
                OBJ = arg.ToJson()
            };
            await BroadcastMessage(obj.ToJson()).ConfigureAwait(false);
        }

        /// <summary>
        /// Broadcasts the last location.
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
        public static async Task BroadcastLastLocation(Device device, Location location)
        {
            var obj = new
            {
                TYPE = "LAST_LOCATION",
                TIME = DateTime.UtcNow,
                OBJ = new { Device = device, Location = location }.ToJson()
            };
            await BroadcastMessage(obj.ToJson()).ConfigureAwait(false);
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
            var obj = new
            {
                TYPE = "LAST_STATUS",
                TIME = DateTime.UtcNow,
                OBJ = new { IMEI = imei , Location = location }.ToJson()
            };
            await BroadcastMessage(obj.ToJson()).ConfigureAwait(false);
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
        internal static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// The broadcast message.
        /// </summary>
        /// <param name="message">The message of the notification.</param>
        /// <returns>The <see cref="Task" />.</returns>
        private static async Task BroadcastMessage(string message)
        {
            if (notifierService == null)
            {
                apiConfiguration = new WinApiConfiguration();
                apiCaller = new ApiCaller();
                notifierService = new Notifier.EndPoints.Service.Base.NotifierService(apiCaller, apiConfiguration);
                IUacService u = new UacService(apiCaller, apiConfiguration);
                uac = new UserAccountService(u);
            }

            var token = await uac.GetToken().ConfigureAwait(false);
            apiCaller.SetJwtToken(token);

            try
            {
                var arg = new SendNotificationArg
                {
                    ApplicationId = 3,
                    Title = "Teltonika AVL",
                    IsSendToAll = true,
                    Body = message,
                    Type = NotificationTimingType.Online
                };

                await notifierService.Notification.SendNotification(arg).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
