using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Map.Models.AVL;

namespace Map.Models.Repositories
{
    public interface IDeviceRepository
    {
        public Task<IEnumerable<Device>> GetAllAsync();
        public Task<Device> GetByIdAsync(int id);
        public Task<Device> GetByIMEIAsync(string imei);
        public Task<Device> SyncAsync(Device device);
    }
}
