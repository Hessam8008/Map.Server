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

using Map.DataAccess.DAO;
using Map.DataAccess.Dapper;
using Map.Models.AVL;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Map.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
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
            const string proc = "[gps].[stpDevice_GetAll]";
            var devices = await this.QueryAsync<DeviceDAO>(proc);
            var result = 
                from d in devices 
                select d.ToDevice();
            return result;
        }

        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task of device.</returns>
        public async Task<Device> GetByIdAsync(int id)
        {
            const string proc = "[gps].[stpDevice_GetById]";
            var param = new { id };
            var device = await this.QueryFirstOrDefaultAsync<DeviceDAO>(proc, param);
            return device.ToDevice();
        }

        /// <summary>
        /// Gets the by IMEI.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <returns>Task of device.</returns>
        public async Task<Device> GetByIMEIAsync(string imei)
        {
            const string proc = "[gps].[stpDevice_GetByIMEI]";
            var param = new { imei };
            var device = await this.QueryFirstOrDefaultAsync<DeviceDAO>(proc, param);
            return device.ToDevice();
        }

        /// <summary>
        /// Synchronizes the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>Task of device.</returns>
        public async Task<Device> SyncAsync(Device device)
        {
            const string proc = "[gps].[stpDevice_Sync]";
            var param = new DeviceDAO(device).DynamicParameters();
            var result = await this.QueryFirstOrDefaultAsync<DeviceDAO>(proc, param);
            return result.ToDevice();
        }
    }
}
