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

using System.Data;
using System.Threading.Tasks;
using Map.Models.AVL;
using Map.Modules.Teltonika.DataAccess.DAO;
using Map.Modules.Teltonika.DataAccess.Dapper;

namespace Map.Modules.Teltonika.DataAccess.Repositories
{
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
            const string proc = "[gps].[stpLocation_Insert]";
            var param = new LocationDAO(location).DynamicParameters();

            await this.ExecuteAsync(proc, param);
            
            return param.Get<int>("ID");
        }
    }
}
