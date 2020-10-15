using Map.EndPoints.Service;
using Map.EndPoints.Service.Models;
using Services.WebApiCaller;
using Services.WebApiCaller.Configuration;
using System;
using System.Threading.Tasks;

namespace Map.Endpoints.Service.Tests.Console
{
    public class Program
    {
        private static async Task Main(string[] _)
        {

            var _caller = new ApiCaller();
            var _config = new WinApiConfiguration();

            _config.Load();

            var ms = new MapService(_caller, _config);

            //await ms.DeviceService.DeleteAsync(70);

            var result = await ms.DeviceService.CreateAsync(new Device()
            {
                IMEI = "123456789",
                Model = "1",
                CreateTime = DateTime.Now,
                OwnerMobileNumber = "91555",
                SN = "134",
                Nickname = "sss",
                SimNumber = "121"
            });

            await ms.DeviceService.UpdateAsync(new Device()
            {
                ID = 71,
                IMEI = "123",
                Model = "1",
                CreateTime = DateTime.Now,
                OwnerMobileNumber = "91555",
                SN = "134",
                Nickname = "sss",
                SimNumber = "121"
            });

            //System.Console.WriteLine(result?.ToString());
        }
    }
}
