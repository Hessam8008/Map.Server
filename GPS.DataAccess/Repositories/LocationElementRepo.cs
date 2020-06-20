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
namespace Map.DataAccess.Repositories
{
    using System.Data;
    using System.Threading.Tasks;

    using Dapper;

    using Map.DataAccess.Abstracts;
    using Map.DataAccess.Gps;

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
            var param = new DynamicParameters();
            param.Add(nameof(element.ID), 0, DbType.Int32, ParameterDirection.InputOutput);
            param.Add(nameof(element.LocationId), element.LocationId);
            param.Add(nameof(element.ElementId), element.ElementId);
            param.Add(nameof(element.ElementValue), element.ElementValue);

            await this.ExecuteAsync("[gps].[stpLocationElement_Insert]", param);
            return param.Get<int>("ID");
        }
    }
}
