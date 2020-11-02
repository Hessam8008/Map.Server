using Map.EndPoints.Service.Args;

namespace Map.EndPoints.Service.Interfaces
{
    using Map.EndPoints.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILocationService
    {
        Task CreateAsync(AddLocationArg location);
        Task<List<Location>> GetLocationsAsync(int deviceId, DateTime from, DateTime to);
    }
}