using Map.Models.AVL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Map.Modules.Teltonika
{
    public interface IBlackBox
    {
        public Task<bool> ApprovedIMEIAsync(string imei);

        public Task<bool> AcceptedLocationsAsync(string imei, List<Location> locations);
    }
}
