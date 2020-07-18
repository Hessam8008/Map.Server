// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="RawData.cs" company="Golriz">
//     Copyright (c) 2020. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;
using Map.DataAccess.Dapper;

namespace Map.Server.Models
{
    /// <summary>
    /// Class RawData.
    /// </summary>
    internal class RawData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawData"/> class.
        /// </summary>
        public RawData()
        {
            this.CreateTime = DateTime.Now;
        }

        public RawData(string imei, string primitiveMessage)
        {
            IMEI = imei;
            PrimitiveMessage = primitiveMessage;
            CreateTime= DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DapperFieldInfo("ID", true, 4, DataType = DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the IMEI.
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the raw data.
        /// </summary>
        /// <value>The raw data.</value>
        public string PrimitiveMessage { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        [DapperIgnoreParameter]
        public DateTime CreateTime { get; set; }
    }
}
