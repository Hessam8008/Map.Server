namespace Map.EndPoints.Service
{
    using System.Collections.Generic;

    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.SubServices;

    using Services.Core;
    using Services.Core.Interfaces;

    public sealed class MapService : IMapService
    {
        private const string _serviceTitle = "Map.Api";

        public MapService(IApiCaller apiService, IApiConfiguration apiConfig)
        {
            apiConfig.Load();

            this.SiteInfo = apiConfig.FindByTitle(_serviceTitle);
            if (this.SiteInfo is null)
                throw new KeyNotFoundException($"Entry for '{_serviceTitle}' not found in the configuration.");

            this.CustomerService = new CustomerService(apiService, this.SiteInfo, "Customer");
        }

        public ICustomerService CustomerService { get; }

        public ApiSite SiteInfo { get; }
    }
}