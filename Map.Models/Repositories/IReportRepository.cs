using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Map.Models.AVL;

namespace Map.Models.Repositories
{
    public interface IReportRepository
    {
        public Task<IEnumerable<Point>> GetLastLocationsAsync(List<int> devices);

        public Task<IEnumerable<AvlPackage>> GetPathAsync(List<int> devices, DateTime from, DateTime to);
    }
}
