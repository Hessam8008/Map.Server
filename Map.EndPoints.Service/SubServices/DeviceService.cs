using Services.Core.Tools;
using System.Net.Http;
using System.Text;

namespace Map.EndPoints.Service.SubServices
{
    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.Models;
    using Services.Core;
    using Services.Core.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class DeviceService : AbstractSubService, IDeviceService
    {
        public DeviceService(IApiCaller apiService, ApiSite site, string root)
            : base(apiService, site, root)
        {
        }
        public async Task<Device> CreateAsync(Device device)
        {
            var body = device.ToJson();
            var param = new RequestParameters
            {
                Path = $"{RootUrl}/Create",
                Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.ApplicationJson)
            };
            var response = await ApiService.PostAsync(param).ConfigureAwait(false);
            return await ParseAsync<Device>(response).ConfigureAwait(false);
        }
        public async Task UpdateAsync(Device device)
        {
            var body = device.ToJson();
            var param = new RequestParameters
            {
                Path = $"{RootUrl}/Update",
                Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.ApplicationJson)
            };
            var response = await ApiService.PutAsync(param).ConfigureAwait(false);
            await ParseAsync(response).ConfigureAwait(false);
        }
        public async Task DeleteAsync(int id)
        {
            var body = id.ToJson();
            var param = new RequestParameters
            {
                Path = $"{RootUrl}/Delete",
                Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.ApplicationJson)
            };
            var response = await ApiService.DeleteAsync(param).ConfigureAwait(false);
            await ParseAsync(response).ConfigureAwait(false);
        }
        public async Task<List<Device>> GetAllAsync()
        {
            var param = new RequestParameters { Path = $"{RootUrl}" };
            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            return await ParseAsync<List<Device>>(response).ConfigureAwait(false);
        }
        public async Task<Device> GetAsync(int id)
        {
            var param = new RequestParameters { Path = $"{RootUrl}/{id}" };
            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            return await ParseAsync<Device>(response).ConfigureAwait(false);
        }
        public async Task<Device> GetByIMEIAsync(string imei)
        {
            var param = new RequestParameters { Path = $"{RootUrl}/GetByIMEI" };
            param.AddUrlParameter("imei", imei);
            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            return await ParseAsync<Device>(response).ConfigureAwait(false);
        }
    }
}