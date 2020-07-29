// ***********************************************************************
// Assembly         : Map.Server
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
namespace Map.Server
{
    using Map.Models;

    /// <summary>
    /// Class DatabaseSettings.
    /// Implements the <see cref="Map.Models.IDatabaseSettings" />
    /// </summary>
    /// <seealso cref="Map.Models.IDatabaseSettings" />
    internal class DatabaseSettings : IDatabaseSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSettings"/> class.
        /// </summary>
        public DatabaseSettings()
        {
#if DEBUG
            this.ConnectionString = "server=dm1server1;uid=dbUser;pwd=1234;database=GPS;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#else
            this.ConnectionString = "server=10.10.1.12\\GCAS;database=GPSTrackerDB;uid=DVP1;pwd=Fly#3592;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#endif
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; }
    }
}