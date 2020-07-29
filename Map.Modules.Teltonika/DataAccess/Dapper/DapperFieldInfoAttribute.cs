// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="DapperFieldInfoAttribute.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.DataAccess.Dapper
{
    using System;
    using System.Data;

    /// <summary>
    /// Class DapperFieldInfoAttribute.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class DapperFieldInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperFieldInfoAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isOutput">if set to <c>true</c> [is output].</param>
        /// <param name="size">The size.</param>
        public DapperFieldInfoAttribute(string fieldName, bool isOutput = false, int size = 0)
        {
            this.FieldName = fieldName;
            this.IsOutput = isOutput;
            this.Size = size;
        }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is output.
        /// </summary>
        /// <value><c>true</c> if this instance is output; otherwise, <c>false</c>.</value>
        public bool IsOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value><c>true</c> if this instance is primary key; otherwise, <c>false</c>.</value>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public DbType DataType { get; set; }
    }
}