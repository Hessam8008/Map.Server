// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="DeviceRepo.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Map.DataAccess.Repositories
{
    using Map.DataAccess.DAO;
    using Map.DataAccess.Dapper;
    using Map.Models.AVL;
    using Map.Models.Repositories;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class DeviceRepo : DapperRepository, IDeviceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceRepo" /> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public DeviceRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>Inserts the asynchronous.</summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException">device</exception>
        public async Task<int> InsertAsync(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            var dao = new DeviceDAO(device);

            var param = dao.DynamicParameters();

            return await QuerySingleOrDefaultAsync<int>("[gps].[stpDevice_Insert]", param);
        }

        /// <summary>Updates the asynchronous.</summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException">device</exception>
        public async Task<int> UpdateAsync(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            var dao = new DeviceDAO(device);

            var param = dao.DynamicParameters();

            return await ExecuteAsync("[gps].[stpDevice_Update]", param);
        }

        /// <summary>Deletes the asynchronous.</summary>
        /// <param name="device"></param>
        /// <exception cref="ArgumentNullException">device</exception>
        public async Task<int> DeleteAsync(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            var param = new { ID = device.ID };

            return await ExecuteAsync("[gps].[stpDevice_Delete]", param);
        }

        /// <summary>
        /// get all as an asynchronous operation.
        /// </summary>
        /// <returns>returns task of enumerable device.</returns>
        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            const string ProcedureName = "[gps].[stpDevice_GetAll]";
            var devices = await QueryAsync<DeviceDAO>(ProcedureName);
            var result =
                from d in devices
                select d?.ToDevice();
            return result;
        }

        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>returns task of device.</returns>
        public async Task<Device> GetByIdAsync(int id)
        {
            const string ProcedureName = "[gps].[stpDevice_GetById]";
            var param = new { id };
            var device = await QueryFirstOrDefaultAsync<DeviceDAO>(ProcedureName, param);
            return device?.ToDevice();
        }

        /// <summary>
        /// Gets the by IMEI.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <returns>returns task of device.</returns>
        public async Task<Device> GetByIMEIAsync(string imei)
        {
            const string ProcedureName = "[gps].[stpDevice_GetByIMEI]";
            var param = new { imei };
            var device = await QueryFirstOrDefaultAsync<DeviceDAO>(ProcedureName, param);
            return device?.ToDevice();
        }

        /// <summary>
        /// Synchronizes the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>returns task of device.</returns>
        public async Task<Device> SyncAsync(Device device)
        {
            const string ProcedureName = "[gps].[stpDevice_Sync]";
            var param = new DeviceDAO(device).DynamicParameters();
            var result = await QueryFirstOrDefaultAsync<DeviceDAO>(ProcedureName, param);
            return result?.ToDevice();
        }
    }
}
