// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="DisconnectedArgs.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace GPS.Modules.Teltonika.Args
{
    /// <summary>
    /// The disconnected args.
    /// </summary>
    public class DisconnectedArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedArgs" /> class.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        public DisconnectedArgs(string imei)
        {
            this.Imei = imei;
        }

        /// <summary>
        /// Gets the IMEI.
        /// </summary>
        /// <value>The IMEI of the device.</value>
        public string Imei { get; }
    }
}