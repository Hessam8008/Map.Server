// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="DeviceRepo.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data;
using System.Threading.Tasks;
using Map.Modules.Teltonika.DataAccess.DAO;
using Map.Modules.Teltonika.DataAccess.Dapper;

namespace Map.Modules.Teltonika.DataAccess.Repositories
{
    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    public class RawDataRepo : DapperRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawDataRepo"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public RawDataRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// Insert a primitive message to the RawData.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// Integer as identity of the record.
        /// </returns>
        public async Task<int> Insert(string imei, string primitiveData)
        {
            var param = new RawDataDAO(imei, primitiveData).DynamicParameters();
            await this.ExecuteAsync("[gps].[stpRawData_Insert]", param);
            return param.Get<int>("ID");
        }
    }
}
