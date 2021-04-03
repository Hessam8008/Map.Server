using Map.EndPoints.Service;
using Map.EndPoints.Service.Models;
using Services.WebApiCaller;
using Services.WebApiCaller.Configuration;
using System;
using System.Threading.Tasks;

namespace Map.Endpoints.Service.Tests.Console
{
    using System.Collections.Generic;

    using Map.EndPoints.Service.Args;

    public class Program
    {
        private static async Task Main(string[] _)
        {
            
            var _caller = new ApiCaller();
            var _config = new WinApiConfiguration();

            _config.Load();

            var ms = new MapService(_caller, _config);

            var param = new AddLocationArg
            {
                Angle = 240,
                DeviceID = 114,
                Speed = 10,
                Time = DateTime.Now,
                Elements = new List<LocationElement> { new LocationElement { Id = 1, Value = new byte[] { 100, 75, 250, 128 } } }
            };

            await ms.LocationService.CreateAsync(param).ConfigureAwait(false);

            //System.Console.WriteLine(result?.ToString());
        }
    }
}
