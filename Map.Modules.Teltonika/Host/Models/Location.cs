// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   The avl data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Map.Modules.Teltonika.Host.Models
{
    /// <summary>
    /// The Location of the device.
    /// </summary>
    internal class Location
    {
        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the Priority.
        /// </summary>
        public Priority Priority { get; set; }

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
        /// Gets or sets the EventIOID.
        /// </summary>
        public byte EventIOID { get; set; }

        /// <summary>
        /// Gets or sets the TotalIOElement.
        /// </summary>
        public byte TotalIOElements { get; set; }

        /// <summary>
        /// Gets or sets the IoElements.
        /// </summary>
        public List<LocationElement> Elements { get; set; }
        

        public Map.Models.AVL.Location ToAvlLocation()
        {
            return new Map.Models.AVL.Location
            {
                Altitude = Altitude,
                Angle = Angle,
                Elements = (from e in Elements select e.ToAvlLocationElement()).ToList(),
                Latitude = Latitude,
                Longitude = Longitude,
                Satellites = Satellites,
                Speed = Speed,
                Time = Time
            };
        }

    }
}
