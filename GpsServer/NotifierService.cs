namespace GpsServer
{
    using System;
    using System.Threading.Tasks;

    using GPS.Modules.Teltonika.Models;

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
        private static INotifierService notifierService;
        private static IApiCaller apiCaller;
        private static IApiConfiguration apiConfiguration;
        private static UserAccountService uac;

        /// <summary>
        /// The broadcast message.
        /// </summary>
        /// <param name="message">
        /// The message of the notification.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
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

        /// <summary>
        /// The Broadcast IMEI.
        /// </summary>
        /// <param name="imei">The IMEI of the device.<see cref="string"/>.</param>
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
        /// The BroadcastPacket.
        /// </summary>
        /// <param name="tcp">The tcp<see cref="TeltonikaTcpPacket"/>.</param>
        public static async Task BroadcastPacket(TcpPacket tcp)
        {
            var obj = new
            {
                TYPE = "PACKET",
                TIME = DateTime.UtcNow,
                OBJ = tcp.ToJson()
            };
            await BroadcastMessage(obj.ToJson()).ConfigureAwait(false);
        }

        /// <summary>
        /// The ToJson.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
