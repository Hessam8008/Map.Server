using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Map.Models.AVL;

namespace Map.Models.Repositories
{
    public interface ILocationRepository
    {
        public Task<int> InsertAsync(Location location);

        public Task<IEnumerable<Location>> GetAsync(int deviceId, DateTime from, DateTime to);

    }
}
