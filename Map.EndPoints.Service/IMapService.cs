namespace Map.EndPoints.Service
{
    using Services.Core;

    public interface IMapService
    {
        ApiSite SiteInfo { get; }

        ICustomerService CustomerService { get; }
    }
}
