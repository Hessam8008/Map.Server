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
using System.Linq;
using System.Net.Sockets;
using Dapper;
using Map.DataAccess.DAO;
using Map.Models.AVL;
using Map.Models.Repositories;

namespace Map.DataAccess.Repositories
{
    using System.Data;
    using System.Threading.Tasks;

    using Dapper;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class LocationRepo : DapperRepository, ILocationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepo"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public LocationRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public async Task<int> InsertAsync(int deviceId, Location location)
        {
            const string proc1 = "[gps].[stpLocation_Insert]";
            const string proc2 = "[gps].[stpLocationElement_Insert]";

            var dao = new LocationDAO(deviceId, location);
            var param1 = dao.DynamicParameters();
            await this.ExecuteAsync(proc1, param1);
            dao.ID = param1.Get<int>("ID");

            if (dao.Elements?.Count > 0)
            {
                foreach (var el in dao.Elements)
                {
                    el.LocationId = dao.ID;
                    var param2 = el.DynamicParameters();
                    await this.ExecuteAsync(proc2, param2);
                    el.ID = param2.Get<int>("ID");
                }
            }

            return dao.ID;


        }

        public async Task<IEnumerable<Location>> GetByDeviceAsync(int deviceId, DateTime @from, DateTime to)
        {
            const string proc = "[gps].[stpLocation_GetByDevice]";
            var reader = await QueryMultipleAsync(proc, new {deviceId, from, to});
            var locations = (await reader.ReadAsync<LocationDAO>()).ToList();
            var elements = (await reader.ReadAsync<LocationElementDAO>()).ToList();

            foreach (var location in locations)
            {
                location.Elements = elements.Where(e => e.LocationId == location.ID).ToList();
            }

            return from l in locations select l.ToLocation();
        }
    }
}
