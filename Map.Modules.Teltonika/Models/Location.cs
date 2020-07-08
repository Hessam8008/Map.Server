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
using Map.Modules.Teltonika.DataAccess.Dapper;

namespace Map.Modules.Teltonika.Models
{
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

        public List<LocationElement> LocationElements { get; set; }

        public Map.Models.AVL.Location ToAvlLocation()
        {
            return new Map.Models.AVL.Location
            {
                Altitude = Altitude,
                Angle = Angle,
                Elements = (from e in LocationElements select e.ToAvlLocationElement()).ToList(),
                Latitude = Latitude,
                Longitude = Longitude,
                Satellites = Satellites,
                Speed = Speed,
                Time = Timestamp
            };
        }

    }
}
