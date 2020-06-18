// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationElement.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   Defines the TeltonikaIoElement type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Map.DataAccess.Gps
{
    /// <summary>
    /// The location element such as fuel, doors, oil, etc.
    /// </summary>
    public class LocationElement
    {
        /// <summary>
        /// Gets or sets the io id..
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets or sets the io value..
        /// </summary>
        public object Value { get; set; }
    }
}
