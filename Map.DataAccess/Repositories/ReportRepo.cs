// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="LocationRepo.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Dapper;

using Map.Models.AVL;
using Map.Models.Repositories;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Map.DataAccess.DAO;
using Map.DataAccess.Dapper;

namespace Map.DataAccess.Repositories
{
    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class ReportRepo : DapperRepository, IReportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepo"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public ReportRepo(IDbTransaction transaction)
            : base(transaction)
        { }
        
        public async Task<IEnumerable<Point>> GetLastLocationsAsync(List<int> devices)
        {
            const string proc = "[gps].[stpReport_GetLastLocations]";
            var reader = await QueryMultipleAsync(proc, new { deviceList = devices.ToDataTable() });
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

        public async Task<IEnumerable<AvlPackage>> GetPathAsync(List<int> devices, DateTime from, DateTime to)
        {
            const string proc = "[gps].[stpReport_GetPath]";
            var reader = await QueryMultipleAsync(proc, new { deviceList = devices.ToDataTable(), from, to });
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
