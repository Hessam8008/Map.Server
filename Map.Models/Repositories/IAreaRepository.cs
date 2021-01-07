using Map.Models.AVL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Map.Models.Repositories
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAsync();

        Task<int> UpdateLocationAsync(int id, float latitude, float longitude);
    }
}
