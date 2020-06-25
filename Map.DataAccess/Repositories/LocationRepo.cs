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

using Dapper;
using Map.DataAccess.DAO;
using Map.Models.AVL;

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

        public async Task<int> Insert(Location location)
        {
            const string proc1 = "[gps].[stpLocation_Insert]";
            const string proc2 = "[gps].[stpLocationElement_Insert]";

            var dao = new LocationDAO(location);
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
    }
}
