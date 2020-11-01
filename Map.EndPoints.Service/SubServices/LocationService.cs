using Map.EndPoints.Service.Args;
using Services.Core.Tools;
using System.Net.Http;
using System.Text;

namespace Map.EndPoints.Service.SubServices
{
    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.Models;
    using Services.Core;
    using Services.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class LocationService : AbstractSubService, ILocationService
    {
        public LocationService(IApiCaller apiService, ApiSite site, string root)
            : base(apiService, site, root)
        {
        }

        //public async Task CreateAsync(Location location)
        //{
        //    var body = location.ToJson();
        //    var param = new RequestParameters
        //    {
        //        Path = $"{RootUrl}/Create",
        //        Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.ApplicationJson)
        //    };
        //    var response = await ApiService.PostAsync(param).ConfigureAwait(false);
        //    await ParseAsync(response).ConfigureAwait(false);
        //}

        public async Task CreateAsync(AddLocationArg location)
        {
            var body = location.ToJson();
            var param = new RequestParameters
            {
                Path = $"{RootUrl}/Create",
                Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.ApplicationJson)
            };
            var response = await ApiService.PostAsync(param).ConfigureAwait(false);
            await ParseAsync(response).ConfigureAwait(false);
        }

        public async Task<List<Location>> GetLocationsAsync(int deviceId, DateTime @from, DateTime to)
        {
            var param = new RequestParameters { Path = $"{RootUrl}/GetLocations" };
            param.AddUrlParameter(nameof(deviceId), deviceId.ToString());
            param.AddUrlParameter(nameof(from), from.ToString("yyyy-MM-dd HH:mm"));
            param.AddUrlParameter(nameof(to), to.ToString("yyyy-MM-dd HH:mm"));

            Console.WriteLine("httpTarget: {0}", param.GetUri());

            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            return await ParseAsync<List<Location>>(response).ConfigureAwait(false);
        }
    }
}