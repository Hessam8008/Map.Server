﻿// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="ClientConnectedArgs.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Map.Models.EventArgs
{
    using System;

    /// <summary>
    /// Defines the <see cref="ClientConnectedArgs" />.
    /// </summary>
    public class ClientConnectedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnectedArgs" /> class.
        /// </summary>
        /// <param name="imei">The IMEI<see cref="string" />.</param>
        public ClientConnectedArgs(string imei)
        {
            this.IMEI = imei;
        }

        /// <summary>
        /// Gets the IMEI.
        /// </summary>
        /// <value>The IMEI of device.</value>
        public string IMEI { get; }
    }
}
