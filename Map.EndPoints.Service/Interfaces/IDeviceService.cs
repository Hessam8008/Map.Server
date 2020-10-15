namespace Map.EndPoints.Service.Interfaces
{
    using Map.EndPoints.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDeviceService
    {
        Task<Device> CreateAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
        Task<List<Device>> GetAllAsync();
        Task<Device> GetAsync(int id);
        Task<Device> GetByIMEIAsync(string imei);
    }
}