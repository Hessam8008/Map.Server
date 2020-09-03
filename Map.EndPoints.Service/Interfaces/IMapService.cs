namespace Map.EndPoints.Service.Interfaces
{
    using Services.Core;

    public interface IMapService
    {
        ApiSite SiteInfo { get; }
        
        ICustomerService CustomerService { get; }
        
        IReportService ReportService { get; }

        IDeviceService DeviceService { get; }

        ILocationService LocationService { get; }


    }
}