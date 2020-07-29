// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="Location.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Map.Modules.Teltonika.DataAccess.Dapper;

    /// <summary>
    /// The Location of the device.
    /// </summary>
    internal class Location
    {
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
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the Priority.
        /// </summary>
        /// <value>The priority.</value>
        public Priority Priority { get; set; }

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
        public byte EventIoId { get; set; }

        /// <summary>
        /// Gets or sets the total io elements.
        /// </summary>
        /// <value>The total io elements.</value>
        public byte TotalIoElements { get; set; }

        /// <summary>
        /// Gets or sets the location elements.
        /// </summary>
        /// <value>The location elements.</value>
        public List<LocationElement> LocationElements { get; set; }

        /// <summary>
        /// Converts to AVL location.
        /// </summary>
        /// <returns>The <see cref="Map.Models.AVL.Location"/>.</returns>
        public Map.Models.AVL.Location ToAvlLocation()
        {
            return new Map.Models.AVL.Location
            {
                Altitude = this.Altitude,
                Angle = this.Angle,
                Elements = (from e in this.LocationElements select e.ToAvlLocationElement()).ToList(),
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Satellites = this.Satellites,
                Speed = this.Speed,
                Time = this.Timestamp
            };
        }
    }
}
