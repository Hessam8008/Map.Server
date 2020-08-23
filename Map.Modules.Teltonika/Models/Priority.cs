// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="Priority.cs" company="Map.Modules.Teltonika">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.Models
{
    /// <summary>
    /// Priority of GPS packet.
    /// </summary>
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
