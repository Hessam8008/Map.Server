namespace Map.EndPoints.Service.Interfaces
{
    using Map.EndPoints.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerService
    {
        Task InsertAsync(CustomerInfo customer);
        Task UpdateAsync(CustomerInfo customer);
        Task DeleteAsync(int id);
        Task<CustomerInfo> GetAsync(int id);
        Task<List<CustomerInfo>> GetByAreaAsync(int area);
    }
}