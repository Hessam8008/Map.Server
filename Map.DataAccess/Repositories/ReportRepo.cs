// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="ReportRepo.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using Map.DataAccess.DAO;
    using Map.DataAccess.Dapper;
    using Map.Models.AVL;
    using Map.Models.Repositories;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class ReportRepo : DapperRepository, IReportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportRepo"/> class. 
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        public ReportRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// get last locations as an asynchronous operation.
        /// </summary>
        /// <param name="devices">The devices.</param>
        /// <returns>Task of IEnumerable Point.</returns>
        public async Task<IEnumerable<Point>> GetLastLocationsAsync(List<int> devices)
        {
            const string ProcedureName = "[gps].[stpReport_GetLastLocations]";
            var reader = await QueryMultipleAsync(ProcedureName, new { deviceList = devices.ToDataTable() });
            var deviceList = (await reader.ReadAsync<DeviceDAO>()).ToList();
            var locationList = (await reader.ReadAsync<LocationDAO>()).ToList();
            var elementList = (await reader.ReadAsync<LocationElementDAO>()).ToList();

            var result = new List<Point>();
            foreach (var daoDevice in deviceList)
            {
                var p = new Point { Device = daoDevice.ToDevice() };

                var daoLocation = locationList.FirstOrDefault(x => x.DeviceId == daoDevice.ID);
                if (daoLocation != null)
                {
                    var daoElements = elementList.Where(x => x.LocationId == daoLocation.ID).ToList();
                    
                    p.Location = daoLocation.ToLocation();
                    p.Location.Elements = (from x in daoElements select x.ToLocationElement()).ToList();
                }

                result.Add(p);
            }

            return result;
        }

        /// <summary>
        /// get path as an asynchronous operation.
        /// </summary>
        /// <param name="devices">The devices.</param>
        /// <param name="from">From time.</param>
        /// <param name="to">To time.</param>
        /// <returns>Task of IEnumerable AVL Package.</returns>
        public async Task<IEnumerable<AvlPackage>> GetPathAsync(List<int> devices, DateTime from, DateTime to)
        {
            const string ProcedureName = "[gps].[stpReport_GetPath]";
            var reader = await this.QueryMultipleAsync(ProcedureName, new { deviceList = devices.ToDataTable(), from, to });
            var deviceList = (await reader.ReadAsync<DeviceDAO>()).ToList();
            var locationList = (await reader.ReadAsync<LocationDAO>()).ToList();

            return deviceList.Select(device => new AvlPackage
            {
                Device = device.ToDevice(),
                Locations = (from location in locationList.Where(l => l.DeviceId == device.ID)
                             select location.ToLocation())
                    .ToList()
            });
        }
    }
}
