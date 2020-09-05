namespace Map.EndPoints.Service.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Models;

    public interface IReportService
    {
        Task<List<Point>> GetLastLocationsAsync(List<int> devices = null);

        Task<List<ProLocation>> BrowseRoute(int device, DateTime start, DateTime end);
    }
}