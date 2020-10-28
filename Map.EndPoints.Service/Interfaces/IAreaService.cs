using Map.EndPoints.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Map.EndPoints.Service.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAllAsync();
    }
}
