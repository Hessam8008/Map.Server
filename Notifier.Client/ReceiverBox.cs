using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Core;
using Services.Core.Interfaces;

namespace Notifier.Client
{
    public class ReceiverBox
    {
        public event OnMessageReceived MessageReceived;


        private readonly IApiConfiguration configuration;
        private readonly IDeviceGenerator deviceGenerator;
        private HubConnection connection;

        public static string Title { get; } = "Notifier.Hub";
        public int DefaultConnectionLimit { get; set; } = 60;
        public string Token { get; set; }


        public ReceiverBox(IApiConfiguration configuration, IDeviceGenerator deviceGenerator)
        {
            this.configuration = configuration;
            this.deviceGenerator = deviceGenerator;
        }

        public async Task StartAsync()
        {
            configuration.Load();
            var apiSite = configuration.FindByTitle(Title);
            if (apiSite == null)
                throw new Exception($"No url address for '{Title}'.");

            var deviceId = deviceGenerator.GetDeviceId();
            connection = new HubConnectionBuilder()
                .WithUrl(new Uri(apiSite.UrlAddress), options =>
                {
                    if (!string.IsNullOrEmpty(Token))
                        options.Headers["Authorization"] = "Bearer " + Token;

                    options.Headers["deviceId"] = deviceId.ToString();
                })
                .WithAutomaticReconnect()
                .Build();
            connection.On<string>("BroadcastMessage", m =>
            {
                MessageReceived?.Invoke(this, new MessageReceivedArgs(m));
            });

            ServicePointManager.DefaultConnectionLimit = DefaultConnectionLimit;
            await connection.StartAsync();
        }

        public async Task StopAsync()
        {
            await connection.StopAsync();
        }

        public async Task HandshakeAsync(object userId)
        {
            await connection.SendCoreAsync("Handshake", new object[] { userId.ToString() });
        }
    }
}
