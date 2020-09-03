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

        public MapService(IApiCaller caller, IApiConfiguration config)
        {
            config.Load();

            this.SiteInfo = config.FindByTitle(_serviceTitle);
            if (this.SiteInfo is null)
            {
                throw new KeyNotFoundException($"Entry for '{_serviceTitle}' not found in the configuration.");
            }

            this.CustomerService = new CustomerService(caller, this.SiteInfo, "Customer");
            this.DeviceService = new DeviceService(caller, this.SiteInfo, "Device");
            this.LocationService = new LocationService(caller, this.SiteInfo, "Location");
            this.ReportService = new ReportService(caller, this.SiteInfo, "Report");
        }

        public ApiSite SiteInfo { get; }

        public ICustomerService CustomerService { get; }

        public IDeviceService DeviceService { get; }

        public ILocationService LocationService { get; }

        public IReportService ReportService { get; }
    }
}