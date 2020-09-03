// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="LocationRepo.cs" company="Golriz">
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

    using global::Dapper;

    using Map.DataAccess.DAO;
    using Map.DataAccess.Dapper;
    using Map.Models.AVL;
    using Map.Models.Repositories;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class LocationRepo : DapperRepository, ILocationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepo" /> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public LocationRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// insert as an asynchronous operation.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="location">The location.</param>
        /// <returns>Task of integer.</returns>
        public async Task<int> InsertAsync(int deviceId, Location location)
        {
            const string Proc1 = "[gps].[stpLocation_Insert]";
            const string Proc2 = "[gps].[stpLocationElement_Insert]";

            var dao = new LocationDAO(deviceId, location);
            var param1 = dao.DynamicParameters();
            await this.ExecuteAsync(Proc1, param1);
            dao.ID = param1.Get<int>("ID");

            if (!(dao.Elements?.Count > 0))
            {
                return dao.ID;
            }

            foreach (var el in dao.Elements)
            {
                el.LocationId = dao.ID;
                var param2 = el.DynamicParameters();
                await this.ExecuteAsync(Proc2, param2);
                el.ID = param2.Get<int>("ID");
            }

            return dao.ID;
        }

        /// <summary>
        /// Get by device and period asynchronously.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="from">From time.</param>
        /// <param name="to">To time.</param>
        /// <returns>returns task of enumerable location.</returns>
        public async Task<IEnumerable<Location>> GetByDeviceAsync(int deviceId, DateTime @from, DateTime to)
        {
            const string Proc = "[gps].[stpLocation_GetByDevice]";
            /*
             * Use DynamicParameters because of using DateTime
             */
            DynamicParameters dp = new DynamicParameters();
            dp.Add("deviceId", deviceId, DbType.Int32);
            dp.Add("from", @from, DbType.DateTime);
            dp.Add("to", to, DbType.DateTime);
            var reader = await this.QueryMultipleAsync(Proc, dp);
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
