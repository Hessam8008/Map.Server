// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="LocationDao.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Map.DataAccess.Dapper;
    using Map.Models.AVL;

    /// <summary>
    /// Class Location.
    /// </summary>
    internal class LocationDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDao"/> class.
        /// </summary>
        public LocationDao()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDao" /> class.
        /// </summary>
        /// <param name="deviceId">Device ID</param>
        /// <param name="location">The location.</param>
        public LocationDao(int deviceId, Location location)
        {
            this.Timestamp = location.Time;
            this.Longitude = location.Longitude;
            this.Latitude = location.Latitude;
            this.Altitude = location.Altitude;
            this.Angle = location.Angle;
            this.Satellites = location.Satellites;
            this.Speed = location.Speed;
            this.DeviceId = deviceId;
            this.Elements = (from e in location.Elements select new LocationElementDao(e)).ToList();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DapperFieldInfo("ID", true, 4, DataType = DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public int DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The altitude.</value>
        public short Altitude { get; set; }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>The angle.</value>
        public short Angle { get; set; }

        /// <summary>
        /// Gets or sets the satellites.
        /// </summary>
        /// <value>The satellites.</value>
        public byte Satellites { get; set; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public short Speed { get; set; }
        
        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        /// <value>The elements.</value>
        [DapperIgnoreParameter]
        public List<LocationElementDao> Elements { get; set; }

        /// <summary>
        /// Converts to location.
        /// </summary>
        /// <returns>return <exception cref="Location">Location</exception>.</returns>
        public Location ToLocation()
        {
            var result = new Location
            {
                Time = this.Timestamp,
                Longitude = this.Longitude,
                Latitude = this.Latitude,
                Altitude = this.Altitude,
                Angle = this.Angle,
                Satellites = this.Satellites,
                Speed = this.Speed
            };

            if (this.Elements != null)
            {
                result.Elements = (from e in this.Elements select e.ToLocationElement()).ToList();
            }

            return result;
        }
    }
}
