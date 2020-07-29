// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="ILocationRepository.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.Models.AVL;

    /// <summary>
    /// Interface ILocationRepository
    /// </summary>
    public interface ILocationRepository
    {
        /// <summary>
        /// Inserts location asynchronously.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="location">The location.</param>
        /// <returns>Task of integer.</returns>
        public Task<int> InsertAsync(int deviceId, Location location);

        /// <summary>
        /// Gets by device asynchronously.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="from">From time.</param>
        /// <param name="to">To time.</param>
        /// <returns>Task of IEnumerable Location.</returns>
        public Task<IEnumerable<Location>> GetByDeviceAsync(int deviceId, DateTime from, DateTime to);
    }
}
