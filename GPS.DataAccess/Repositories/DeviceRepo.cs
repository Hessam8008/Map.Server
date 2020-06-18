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
namespace Map.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using Map.DataAccess.Abstracts;
    using Map.DataAccess.Gps;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="Map.DataAccess.Abstracts.DapperRepository" />
    /// </summary>
    /// <seealso cref="Map.DataAccess.Abstracts.DapperRepository" />
    public class DeviceRepo : DapperRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceRepo"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public DeviceRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>
        /// get all as an asynchronous operation.
        /// </summary>
        /// <returns>IEnumerable Task of device</returns>
        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await this.QueryAsync<Device>("[gps].[stpDevice_GetAll]");
        }

        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task of device.</returns>
        public async Task<Device> GetByIdAsync(int id)
        {
            return await this.QueryFirstOrDefaultAsync<Device>("[gps].[stpDevice_GetById]", new { id });
        }

        /// <summary>
        /// Gets the by IMEI.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <returns>Task of device.</returns>
        public async Task<Device> GetByIMEI(long imei)
        {
            return await this.QueryFirstOrDefaultAsync<Device>("[gps].[stpDevice_GetByIMEI]", new { imei });
        }

        /// <summary>
        /// Synchronizes the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>Task of device.</returns>
        public async Task<Device> Sync(Device device)
        {
            return await this.QueryFirstOrDefaultAsync<Device>("[gps].[stpDevice_Sync]", device);
        }
    }
}
