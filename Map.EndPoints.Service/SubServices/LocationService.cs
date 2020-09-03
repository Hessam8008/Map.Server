namespace Map.EndPoints.Service.SubServices
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.Models;

    using Services.Core;
    using Services.Core.Interfaces;

    internal class LocationService : AbstractSubService, ILocationService
    {
        public LocationService(IApiCaller apiService, ApiSite site, string root)
            : base(apiService, site, root)
        {
        }

        public async Task<List<Location>> GetPathAsync(int deviceId, DateTime @from, DateTime to)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/GetPath" };
            param.AddUrlParameter(nameof(deviceId), deviceId.ToString());
            param.AddUrlParameter(nameof(from), from.ToString("yyyy-MM-dd HH:mm"));
            param.AddUrlParameter(nameof(to), to.ToString("yyyy-MM-dd HH:mm"));
            
            Console.WriteLine("httpTarget: {0}", param.GetUri());

            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<List<Location>>(response).ConfigureAwait(false);
        }
    }
}