// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="LocationElementRepo.cs" company="Golriz">
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
    /// Class LocationElementRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    public class LocationElementRepo : DapperRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationElementRepo"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public LocationElementRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// Insert a location element.
        /// </summary>
        /// <param name="element">
        /// The data.
        /// </param>
        /// <returns>
        /// Integer as identity of the record.
        /// </returns>
        public async Task<int> Insert(LocationElement element)
        {
            const string proc = "[gps].[stpLocationElement_Insert]";
            var param = new LocationElementDAO(element).DynamicParameters();

            await this.ExecuteAsync(proc, param);

            return param.Get<int>("ID");
        }
    }
}
