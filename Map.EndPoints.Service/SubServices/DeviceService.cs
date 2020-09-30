namespace Map.EndPoints.Service.SubServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.Models;

    using Services.Core;
    using Services.Core.Interfaces;

    internal class DeviceService : AbstractSubService, IDeviceService
    {
        public DeviceService(IApiCaller apiService, ApiSite site, string root)
            : base(apiService, site, root)
        {
        }

        public async Task<List<Device>> GetAllAsync()
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}" };
            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<List<Device>>(response).ConfigureAwait(false);
        }

        public async Task<Device> GetAsync(int id)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/{id}" };
            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<Device>(response).ConfigureAwait(false);
        }

        public async Task<Device> GetByIMEIAsync(string imei)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/GetByIMEI" };
            param.AddUrlParameter("imei", imei);
            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<Device>(response).ConfigureAwait(false);
        }
    }
}