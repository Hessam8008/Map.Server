﻿using System;
using System.Threading.Tasks;
using Map.Client.Interfaces;
using Map.Client.Models;
using Map.Client.Services;
using Newtonsoft.Json;
using Notifier.Client.Libs.Windows.Services;
using Services.Core;
using Services.Core.Interfaces;
using Services.WebApiCaller.Configuration;
using Utilities.LocalStorage.LocalStorage;

namespace Map.Client
{
    internal class Program
    {
        /// <summary>
        /// The token of notification system registered in the notification server.
        /// notification server: http://10.220.1.2:5050/index.html#/
        /// </summary>
        private const string NotifierToken = "b3648cd2a9";

        public static async Task Main(string[] _)
        {
            IApiConfiguration configuration = new WinApiConfiguration();
            configuration.Load();
            ApiSite apiSite = configuration.FindByTitle("Notifier.Hub");
            ShowMessage($"Hub: {apiSite.UrlAddress}", ConsoleColor.Gray);
            ILocalStorage localSetting = new LiteDBLocalStorage();
            localSetting.Initialize(".\\","signalR");
            INotifierService notifierService = new NotifierService(localSetting);
            notifierService.Initialize(NotifierToken, apiSite.UrlAddress);


            IMessageParser messageParser = new MessageParser2();

            notifierService.OnReceiveNotification += (sender, e) =>
                {
                    try
                    {
                        var plainMessage = JsonConvert.DeserializeObject<PlainMessage>(e.Body);
                        ShowMessage($"Time: {plainMessage.Time.ToLocalTime()}, Type: {plainMessage.Type}", ConsoleColor.Magenta);
                        var message = messageParser.Parse(plainMessage);

                        if (message == null)
                        {
                            ShowMessage($"Unknown message:\n{e.Body}", ConsoleColor.DarkMagenta);
                        }
                        else
                        {
                            message?.PrintResult();
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(e.Body);
                        Console.WriteLine(exception);
                    }
                };



            await notifierService.Connect();
            Console.WriteLine("Receiver has been started.");
            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
            await notifierService.Disconnect();

            Console.WriteLine("Receiver has been stopped.");
            Console.ReadKey();

        }


        private static void ShowMessage(string message, ConsoleColor foreColor)
        {
            var temp_foreColor = Console.ForegroundColor;
            Console.ForegroundColor = foreColor;
            Console.WriteLine($"* {message}");
            Console.ForegroundColor = temp_foreColor;
        }

    }
}