namespace Map.EndPoints.Service.Interfaces
{
    using Map.EndPoints.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDeviceService
    {
        Task<int> CreateAsync(Device device);
        Task<int> UpdateAsync(Device device);
        Task<int> DeleteAsync(Device device);
        Task<List<Device>> GetAllAsync();
        Task<Device> GetAsync(int id);
        Task<Device> GetByIMEIAsync(string imei);
    }
}