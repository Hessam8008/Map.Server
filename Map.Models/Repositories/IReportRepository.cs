// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="IReportRepository.cs" company="Golriz">
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
    /// Interface IReportRepository
    /// </summary>
    public interface IReportRepository
    {
        /// <summary>
        /// Gets the last locations asynchronous.
        /// </summary>
        /// <param name="devices">The devices.</param>
        /// <returns>Task of IEnumerable Point.</returns>
        public Task<IEnumerable<Point>> GetLastLocationsAsync(List<int> devices);

        /// <summary>
        /// Gets the path asynchronously.
        /// </summary>
        /// <param name="devices">The devices.</param>
        /// <param name="from">From time.</param>
        /// <param name="to">To time.</param>
        /// <returns>Task of IEnumerable AVL Package.</returns>
        public Task<IEnumerable<AvlPackage>> GetPathAsync(List<int> devices, DateTime from, DateTime to);
    }
}
