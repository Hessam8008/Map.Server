// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="LocationElement.cs" company="Map.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.Models
{
    /// <summary>
    /// The location element such as fuel, doors, oil, etc.
    /// </summary>
    internal class LocationElement
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

        /// <summary>
        /// Converts to AVL location element.
        /// </summary>
        /// <returns>returns <exception cref="Map.Models.AVL.LocationElement"></exception>.</returns>
        public Map.Models.AVL.LocationElement ToAvlLocationElement()
        {
            return new Map.Models.AVL.LocationElement
            {
                Id = this.Id,
                Value = this.Value
            };
        }
    }
}
