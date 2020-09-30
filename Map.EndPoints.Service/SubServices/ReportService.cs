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

    internal class ReportService : AbstractSubService, IReportService
    {
        public ReportService(IApiCaller apiService, ApiSite site, string root)
            : base(apiService, site, root)
        {
        }

        public async Task<List<Point>> GetLastLocationsAsync(List<int> devices = null)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/GetLastLocations" };

            if (devices != null)
            {
                foreach (var device in devices)
                {
                    param.AddUrlParameter("devices", device.ToString());
                }
            }

            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<List<Point>>(response).ConfigureAwait(false);
        }

        public async Task<List<ProLocation>> BrowseRoute(int device, DateTime start, DateTime end)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/BrowseRoute" };
            param.AddUrlParameter("deviceId", device.ToString());
            param.AddUrlParameter("from", start.ToString(CultureInfo.InvariantCulture));
            param.AddUrlParameter("to", end.ToString(CultureInfo.InvariantCulture));

            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<List<ProLocation>>(response).ConfigureAwait(false);
        }
    }
}