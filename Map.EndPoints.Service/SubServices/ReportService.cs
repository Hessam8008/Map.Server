namespace Map.EndPoints.Service.SubServices
{
    using System.Collections.Generic;
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
    }
}