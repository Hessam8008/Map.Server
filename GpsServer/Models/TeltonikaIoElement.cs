// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TeltonikaIoElement.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   Defines the TeltonikaIoElement type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GpsServer.Models
{
    /// <summary>
    /// The Teltonika IO element.
    /// </summary>
    public class TeltonikaIoElement
    {
        /// <summary>
        /// Gets or sets the io id..
        /// </summary>
        public byte IoID { get; set; }

        /// <summary>
        /// Gets or sets the io value..
        /// </summary>
        public object IoValue { get; set; }
    }
}
