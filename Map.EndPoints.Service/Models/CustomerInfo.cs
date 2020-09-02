// ***********************************************************************
// Assembly         : Map.EndPoints.Service
// Author           : U12178
// Created          : 09-01-2020
//
// Last Modified By : U12178
// Last Modified On : 09-01-2020
// ***********************************************************************
// <copyright file="CustomerInfo.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Map.EndPoints.Service.Models
{
    /// <summary>
    ///     The customer information.
    /// </summary>
    public class CustomerInfo
    {
        /// <summary>
        ///     Gets or sets a value indicating whether active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        ///     Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Gets or sets the area (Branch).
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        public byte Grade { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Gets or sets the Latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        ///     Gets or sets the Longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}