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
            : base(apiService, site, root)
        {
        }

        public async Task<List<CustomerInfo>> GetByArea(int area)
        {
            var param = new RequestParameters { Path = $"{this.RootUrl}/GetByArea" };

            param.AddUrlParameter(nameof(area), area.ToString());
            var response = await this.ApiService.GetAsync(param).ConfigureAwait(false);
            return await this.ParseAsync<List<CustomerInfo>>(response).ConfigureAwait(false);
        }
    }
}