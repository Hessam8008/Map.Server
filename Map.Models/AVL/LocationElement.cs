// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="LocationElement.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.AVL
{
    /// <summary>
    /// The location element such as fuel, doors, oil, etc.
    /// </summary>
    public class LocationElement
    {
        /// <summary>
        /// Gets or sets the io id..
        /// </summary>
        /// <value>The identifier.</value>
        public byte Id { get; set; }

        /// <summary>
        /// Gets or sets the io value..
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }
    }
}
