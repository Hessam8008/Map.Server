namespace Map.EndPoints.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Models;

    public interface IReportService
    {
        Task<List<Point>> GetLastLocationsAsync(List<int> devices = null);
    }
}