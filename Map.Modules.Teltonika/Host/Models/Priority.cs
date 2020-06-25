// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Priority.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   Defines the Priority type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Map.Modules.Teltonika.Host.Models
{
    /// <summary>Priority of GPS packet.</summary>
    internal enum Priority : byte
    {
        /// <summary>
        /// The low.
        /// </summary>
        Low = 0,

        /// <summary>
        /// The high.
        /// </summary>
        High = 1,

        /// <summary>
        /// The panic.
        /// </summary>
        Panic = 2,

        /// <summary>
        /// The security.
        /// </summary>
        Security = 3
    }
}
