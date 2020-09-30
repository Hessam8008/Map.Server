namespace Map.EndPoints.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Models;

    public interface IDeviceService
    {
        Task<List<Device>> GetAllAsync();
        
        Task<Device> GetAsync(int id);
        
        Task<Device> GetByIMEIAsync(string imei);
    }
}