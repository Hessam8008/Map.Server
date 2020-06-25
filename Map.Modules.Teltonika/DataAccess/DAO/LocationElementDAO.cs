// ***********************************************************************
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

using Map.Modules.Teltonika.DataAccess.Dapper;

namespace Map.Modules.Teltonika.DataAccess.DAO
{
    /// <summary>
    /// Class LocationElement.
    /// </summary>
    public class LocationElementDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationElementDAO"/> class.
        /// </summary>
        public LocationElementDAO()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationElementDAO"/> class.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        public LocationElementDAO(Models.AVL.LocationElement element)
        {
            this.ElementId = element.Id;
            this.ElementValue = element.Value;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DapperFieldInfo("ID", true)]
        public int ID { get; set; }

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
        public object ElementValue { get; set; }
    }
}
