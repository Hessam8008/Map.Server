namespace Map.EndPoints.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerService
    {
        Task<List<CustomerInfo>> GetByArea(int area);
    }
}