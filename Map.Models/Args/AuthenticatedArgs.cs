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
namespace Map.Models.Args
{
    /// <summary>
    /// Defines the <see cref="ClientConnectedArgs" />.
    /// </summary>
    public class ClientConnectedArgs
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
