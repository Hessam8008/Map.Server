namespace Map.EndPoints.Service.Interfaces
{
    using Services.Core;

    public interface IMapService
    {
        ICustomerService CustomerService { get; }

        ApiSite SiteInfo { get; }
    }
}