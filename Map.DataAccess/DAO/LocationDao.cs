// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="Location.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Map.DataAccess.Dapper;
using Map.Models.AVL;

namespace Map.DataAccess.DAO
{
    /// <summary>
    /// Class Location.
    /// </summary>
    internal class LocationDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDAO"/> class.
        /// </summary>
        public LocationDAO()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDAO"/> class.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        public LocationDAO(Location location)
        {
            this.Timestamp = location.Time;
            this.Longitude = location.Longitude;
            this.Latitude = location.Latitude;
            this.Altitude = location.Altitude;
            this.Angle = location.Angle;
            this.Satellites = location.Satellites;
            this.Speed = location.Speed;
            this.DeviceId = location.Device.ID;
            this.Elements = (from e in location.Elements select new LocationElementDAO(e)).ToList();
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

     
        [DapperIgnoreParameter]
        public List<LocationElementDAO> Elements { get; set; }


        public Location ToLocation()
        {
            return new Location
            {
                Time = Timestamp,
                Longitude = Longitude,
                Latitude = Latitude,
                Altitude = Altitude,
                Angle = Angle,
                Satellites = Satellites,
                Speed = Speed
            };
        }

    }
}
