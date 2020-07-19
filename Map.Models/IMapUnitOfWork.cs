using System;
using System.Collections.Generic;
using System.Text;
using Map.Models.Repositories;

namespace Map.Models
{
    public interface IMapUnitOfWork
    {
        public IDeviceRepository DeviceRepository { get; }
        public ILocationRepository LocationRepository { get; }
        public IReportRepository ReportRepository { get; }
    }
}
