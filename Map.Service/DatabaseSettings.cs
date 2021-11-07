// ***********************************************************************
// Assembly         : Map.Service
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="DatabaseSettings.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.Extensions.Configuration;

namespace Map.Service
{
    using Models;

    /// <summary>
    /// Class DatabaseSettings.
    /// Implements the <see cref="Map.Models.IDatabaseSettings" />
    /// </summary>
    /// <seealso cref="Map.Models.IDatabaseSettings" />
    internal class DatabaseSettings : IDatabaseSettings
    {
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSettings"/> class.
        /// </summary>
        public DatabaseSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ConnectionString = configuration["ConnectionString"];
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; }
    }
}