// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="IBlackBox.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.Models.AVL;

    /// <summary>
    /// Interface IBlackBox
    /// </summary>
    public interface IBlackBox
    {
        /// <summary>
        /// Approved the IMEI asynchronous.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <returns>The task of boolean.</returns>
        public Task<bool> ApprovedIMEIAsync(string imei);

        /// <summary>
        /// Accepted the locations asynchronously.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <param name="locations">The locations.</param>
        /// <returns>The task of boolean.</returns>
        public Task<bool> AcceptedLocationsAsync(string imei, List<Location> locations);
    }
}
