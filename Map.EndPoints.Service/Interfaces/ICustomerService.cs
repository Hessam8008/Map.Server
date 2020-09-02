namespace Map.EndPoints.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Models;

    public interface ICustomerService
    {
        Task<List<CustomerInfo>> GetByArea(int area);
    }
}