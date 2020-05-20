// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TeltonikaAvlData.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   The teltonika avl data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GpsServer.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The teltonika avl data.
    /// </summary>
    public class TeltonikaAvlData
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
        public byte TotalIOElement { get; set; }

        /// <summary>
        /// Gets or sets the IoElements.
        /// </summary>
        public List<TeltonikaIoElement> IoElements { get; set; }
    }
}
