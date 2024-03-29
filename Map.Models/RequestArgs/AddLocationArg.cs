﻿using Map.Models.AVL;
using System;
using System.Collections.Generic;

namespace Map.Models.RequestArgs
{
    public class AddLocationArg
    {
        /// <summary>Gets or sets the device identifier.</summary>
        /// <value>The device identifier.</value>
        public int DeviceID { get; set; }
        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Altitude.
        /// </summary>
        /// <value>The altitude.</value>
        public short Altitude { get; set; }

        /// <summary>
        /// Gets or sets the Angle.
        /// </summary>
        /// <value>The angle.</value>
        public short Angle { get; set; }

        /// <summary>
        /// Gets or sets the Satellites.
        /// </summary>
        /// <value>The satellites.</value>
        public byte Satellites { get; set; }

        /// <summary>
        /// Gets or sets the Speed.
        /// </summary>
        /// <value>The speed.</value>
        public short Speed { get; set; }

        /// <summary>
        /// Gets or sets the IoElements.
        /// </summary>
        /// <value>The elements.</value>
        public List<LocationElement> Elements { get; set; }

        public Location ToLocation()
        {
            var location = new Location()
            {
                Longitude = Longitude,
                Latitude = Latitude,
                Altitude = Altitude,
                Angle = Angle,
                Elements = Elements,
                Satellites = Satellites,
                Speed = Speed,
                Time = Time
            };

            return location;
        }

    }
}
