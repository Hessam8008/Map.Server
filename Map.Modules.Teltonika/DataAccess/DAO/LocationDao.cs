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
using Map.Models.AVL;
using Map.Modules.Teltonika.DataAccess.Dapper;

namespace Map.Modules.Teltonika.DataAccess.DAO
{
    /// <summary>
    /// Class Location.
    /// </summary>
    public class LocationDAO
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
            this.Priority = (byte)location.Priority;
            this.Longitude = location.Longitude;
            this.Latitude = location.Latitude;
            this.Altitude = location.Altitude;
            this.Angle = location.Angle;
            this.Satellites = location.Satellites;
            this.Speed = location.Speed;
            this.EventIo = location.EventIOID;
            this.TotalIoElements = location.TotalIOElements;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DapperFieldInfo("ID", true)]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the RawData identifier.
        /// </summary>
        /// <value>The RawData identifier.</value>
        public int RawDataID { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public int DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the codec.
        /// </summary>
        /// <value>The codec.</value>
        public byte Codec { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public byte Priority { get; set; }

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
        /// Gets or sets the event io.
        /// </summary>
        /// <value>The event io.</value>
        public byte EventIo { get; set; }

        /// <summary>
        /// Gets or sets the total io elements.
        /// </summary>
        /// <value>The total io elements.</value>
        public byte TotalIoElements { get; set; }

        public Location ToLocation()
        {
            return new Location
            {
                Time = Timestamp,
                Priority = (Priority) Priority,
                Longitude = Longitude,
                Latitude = Latitude,
                Altitude = Altitude,
                Angle = Angle,
                Satellites = Satellites,
                Speed = Speed,
                EventIOID = EventIo,
                TotalIOElements = TotalIoElements
            };
        }

    }
}
