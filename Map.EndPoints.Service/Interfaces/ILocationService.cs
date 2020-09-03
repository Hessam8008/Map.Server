namespace Map.EndPoints.Service.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Models;

    public interface ILocationService
    {
        Task<List<Location>> GetPathAsync(int device, DateTime from, DateTime to);
    }
}