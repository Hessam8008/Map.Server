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
namespace Map.DataAccess.Gps
{
    using System;

    /// <summary>
    /// Class Location.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the log identifier.
        /// </summary>
        /// <value>The log identifier.</value>
        public int LogId { get; set; }

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
    }
}
