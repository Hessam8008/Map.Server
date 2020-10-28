using System.Net.Http;
using System.Text;
using Services.Core.Tools;

namespace Map.EndPoints.Service.SubServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.Models;

    using Services.Core;
    using Services.Core.Interfaces;

    internal class CustomerService : AbstractSubService, ICustomerService
    {
        public CustomerService(IApiCaller apiService, ApiSite site, string root)
            : base(apiService, site, root) { }

        public async Task InsertAsync(CustomerInfo customer)
        {
            var body = customer.ToJson();
            var param = new RequestParameters
            {
                Path = $"{RootUrl}/Create",
                Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.ApplicationJson)
            };
            var response = await ApiService.PostAsync(param).ConfigureAwait(false);
            await ParseAsync(response).ConfigureAwait(false);
        }
        
        public async Task UpdateAsync(CustomerInfo customer)
        {
            var body = customer.ToJson();
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
            var param = new RequestParameters
            {
                Path = $"{RootUrl}/Delete",
            };

            param.AddUrlParameter(nameof(id), id.ToString());

            var response = await ApiService.DeleteAsync(param).ConfigureAwait(false);
            await ParseAsync(response).ConfigureAwait(false);
        }
        
        public async Task<CustomerInfo> GetAsync(int id)
        {
            var param = new RequestParameters { Path = $"{RootUrl}/{id}" };
            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            return await ParseAsync<CustomerInfo>(response).ConfigureAwait(false);
        }

        public async Task<List<CustomerInfo>> GetByAreaAsync(int area)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/GetByArea" };

            param.AddUrlParameter(nameof(area), area.ToString());
            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<List<CustomerInfo>>(response).ConfigureAwait(false);
        }

        public async Task GetChangesAsync()
        {
            var param = new RequestParameters { Path = $"{RootUrl}/GetChanges" };
            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            await ParseAsync(response).ConfigureAwait(false);
        }
    }
}