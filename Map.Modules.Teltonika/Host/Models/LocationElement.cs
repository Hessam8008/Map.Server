// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationElement.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   Defines the TeltonikaIoElement type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Map.Modules.Teltonika.Host.Models
{
    /// <summary>
    /// The location element such as fuel, doors, oil, etc.
    /// </summary>
    internal class LocationElement
    {
        /// <summary>
        /// Gets or sets the io id..
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets or sets the io value..
        /// </summary>
        public object Value { get; set; }


        public Map.Models.AVL.LocationElement ToAvlLocationElement()
        {
            return new Map.Models.AVL.LocationElement
            {
                Id = Id,
                Value = Value
            };
        }

    }
}
