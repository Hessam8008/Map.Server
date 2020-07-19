// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   The avl data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Map.Models.AVL
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Location of the device.
    /// </summary>
    public class Location
    {
        //public Device Device { get; set; }
        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Altitude.
        /// </summary>
        public short Altitude { get; set; }

        /// <summary>
        /// Gets or sets the Angle.
        /// </summary>
        public short Angle { get; set; }

        /// <summary>
        /// Gets or sets the Satellites.
        /// </summary>
        public byte Satellites { get; set; }

        /// <summary>
        /// Gets or sets the Speed.
        /// </summary>
        public short Speed { get; set; }
        
        /// <summary>
        /// Gets or sets the IoElements.
        /// </summary>
        public List<LocationElement> Elements { get; set; }
    }
}
