using System;
using System.Net.Http;
using GpsServer.Models;

namespace GpsServer.Teltonika.Server
{
    public static class MessageCenter
    {
        //private static string url = "http://94.183.243.23:4054/api/GlobalHub/Broadcast";
        private static string url = "http://10.10.1.34:4054/api/GlobalHub/Broadcast";
        private static HttpClient client;
        private static void BroadcastMessage(string msg)
        {
            client ??= new HttpClient();

            try
            {
                var newUrl = $"{url}?Message={Uri.EscapeUriString(msg)}";

                var response = client.PostAsync(newUrl, null).ConfigureAwait(false).GetAwaiter().GetResult();

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception($"Send notification failed. Status code = {response.StatusCode}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void BroadcastIMEI(string msg)
        {
            var obj = new 
            {
                TYPE = "IMEI",
                TIME = DateTime.UtcNow,
                OBJ = msg
            };
            BroadcastMessage(obj.ToJson());
        }

        public static void BroadcastPacket(TeltonikaTcpPacket tcp)
        {
            var obj = new
            {
                TYPE = "PACKET",
                TIME = DateTime.UtcNow,
                OBJ = tcp.ToJson()
            };
            BroadcastMessage(obj.ToJson());
        }

        public static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

    }
}
