// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="LocationElementDAO.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess.DAO
{
    using System.Data;

    using Map.DataAccess.Dapper;
    using Map.Models.AVL;

    /// <summary>
    /// Class LocationElement.
    /// </summary>
    internal class LocationElementDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationElementDao" /> class.
        /// </summary>
        public LocationElementDao()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationElementDao" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public LocationElementDao(LocationElement element)
        {
            this.ElementId = element.Id;
            this.ElementValue = element.Value;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The identifier.</value>
        [DapperFieldInfo("ID", true, DataType = DbType.Int32)]
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

        /// <summary>
        /// Converts to location element.
        /// </summary>
        /// <returns>returns the <exception cref="LocationElement">LocationElement</exception>.</returns>
        public LocationElement ToLocationElement()
        {
            return new LocationElement
            {
                Id = this.ElementId,
                Value = this.ElementValue
            };
        }
    }
}
