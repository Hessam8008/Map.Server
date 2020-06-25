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
using Map.Modules.Teltonika.DataAccess.Dapper;

namespace Map.Modules.Teltonika.DataAccess.DAO
{
    /// <summary>
    /// Class RawData.
    /// </summary>
    public class RawDataDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawDataDAO"/> class.
        /// </summary>
        public RawDataDAO()
        {
            this.CreateTime = DateTime.Now;
        }

        public RawDataDAO(string imei, string primitiveMessage)
        {
            IMEI = imei;
            PrimitiveMessage = primitiveMessage;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DapperFieldInfo("ID", true)]
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
