// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="RawDataRepo.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.DataAccess.Repositories
{
    using System.Data;
    using System.Threading.Tasks;

    using Map.Modules.Teltonika.DataAccess.Dapper;
    using Map.Modules.Teltonika.Models;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class RawDataRepo : DapperRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawDataRepo" /> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public RawDataRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// Insert a primitive message to the RawData.
        /// </summary>
        /// <param name="imei">IMEI of the device.</param>
        /// <param name="primitiveData">Hex string.</param>
        /// <returns>Integer as identity of the record.</returns>
        public async Task<int> Insert(string imei, string primitiveData)
        {
            var rawData = new RawData(imei, primitiveData);
            var result = await this.Insert(rawData);
            return result;
        }

        /// <summary>
        /// Inserts the specified raw data.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <returns>Returns task of integer.</returns>
        public async Task<int> Insert(RawData rawData)
        {
            var param = rawData.DynamicParameters();
            await this.ExecuteAsync("[teltonika].[stpRawData_Insert]", param);
            return param.Get<int>("ID");
        }
    }
}
