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
namespace Map.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using Dapper;

    using Map.DataAccess.Abstracts;
    using Map.DataAccess.Gps;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    public class LocationRepo : DapperRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepo"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public LocationRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// Insert a location.
        /// </summary>
        /// <param name="location">
        /// The data.
        /// </param>
        /// <returns>
        /// Integer as identity of the record.
        /// </returns>
        public async Task<int> Insert(Location location)
        {
            var param = new DynamicParameters();
            param.Add(nameof(Location.ID), 0, DbType.Int32, ParameterDirection.InputOutput);
            param.Add(nameof(Location.RawDataID), location.RawDataID);
            param.Add(nameof(Location.DeviceId), location.DeviceId);
            param.Add(nameof(Location.Codec), location.Codec);
            param.Add(nameof(Location.Timestamp), location.Timestamp);
            param.Add(nameof(Location.Priority), location.Priority);
            param.Add(nameof(Location.Longitude), location.Longitude);
            param.Add(nameof(Location.Latitude), location.Latitude);
            param.Add(nameof(Location.Altitude), location.Altitude);
            param.Add(nameof(Location.Angle), location.Angle);
            param.Add(nameof(Location.Satellites), location.Satellites);
            param.Add(nameof(Location.Speed), location.Speed);
            param.Add(nameof(Location.EventIo), location.EventIo);
            param.Add(nameof(Location.TotalIoElements), location.TotalIoElements);

            await this.ExecuteAsync("[gps].[stpLocation_Insert]", param);
            return param.Get<int>("ID");
        }
    }
}
