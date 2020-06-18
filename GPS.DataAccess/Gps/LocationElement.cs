﻿// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="LocationElement.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess.Gps
{
    /// <summary>
    /// Class LocationElement.
    /// </summary>
    public class LocationElement
    {
        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>The location identifier.</value>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        /// <value>The element identifier.</value>
        public byte ElementId { get; set; }

        /// <summary>
        /// Gets or sets the element value.
        /// </summary>
        /// <value>The element value.</value>
        public long? ElementValue { get; set; }
    }
}
