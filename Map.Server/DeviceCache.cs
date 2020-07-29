// ***********************************************************************
// Assembly         : Map.Server
// Author           : U12178
// Created          : 07-29-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="DeviceCache.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Server
{
    using Map.Models.AVL;

    /// <summary>
    /// Class DeviceCache.
    /// </summary>
    internal class DeviceCache
    {
        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>The device.</value>
        public Device Device { get; set; }

        /// <summary>
        /// Gets or sets the last location.
        /// </summary>
        /// <value>The last location.</value>
        public Location LastLocation { get; set; }

        /// <summary>
        /// Gets or sets the last status.
        /// </summary>
        /// <value>The last status.</value>
        public Location LastStatus { get; set; }
    }
}