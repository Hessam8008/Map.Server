// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="AuthenticatedArgs.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace GPS.Modules.Teltonika.Args
{
    /// <summary>
    /// Defines the <see cref="AuthenticatedArgs" />.
    /// </summary>
    public class AuthenticatedArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedArgs" /> class.
        /// </summary>
        /// <param name="imei">The IMEI<see cref="string" />.</param>
        public AuthenticatedArgs(string imei)
        {
            this.Imei = imei;
        }

        /// <summary>
        /// Gets the IMEI.
        /// </summary>
        /// <value>The IMEI of device.</value>
        public string Imei { get; }

        /// <summary>
        /// Gets or sets a value indicating whether Accepted.
        /// </summary>
        /// <value><c>true</c> if accepted; otherwise, <c>false</c>.</value>
        public bool Accepted { get; set; } = false;
    }
}
