// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="IDeviceRepository.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.Repositories
{
    using Map.Models.AVL;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface IDeviceRepository
    /// </summary>
    public interface IDeviceRepository
    {
        /// <summary>Inserts the asynchronous.</summary>
        /// <param name="device">The device.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> InsertAsync(Device device);

        /// <summary>Updates the asynchronous.</summary>
        /// <param name="device">The device.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> UpdateAsync(Device device);

        /// <summary>Deletes the asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Task of IEnumerable Device.</returns>
        public Task<IEnumerable<Device>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task of Device.</returns>
        public Task<Device> GetByIdAsync(int id);

        /// <summary>
        /// Gets device by IMEI asynchronously.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <returns>Task of Device.</returns>
        public Task<Device> GetByIMEIAsync(string imei);

        /// <summary>
        /// Synchronizes the asynchronous.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>Task of Device.</returns>
        public Task<Device> SyncAsync(Device device);
    }
}
