// ***********************************************************************
// Assembly         : GPS.Modules.Teltonika
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 06-15-2020
// ***********************************************************************
// <copyright file="ErrorArgs.cs" company="GPS.Modules.Teltonika">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace GPS.Modules.Teltonika.Args
{
    using System;

    /// <summary>
    /// Defines the <see cref="ErrorArgs" />.
    /// </summary>
    public class ErrorArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorArgs"/> class.
        /// </summary>
        /// <param name="error">
        /// The error<see cref="string"/>.
        /// </param>
        /// <param name="imei">
        /// The IMEI.
        /// </param>
        public ErrorArgs(string error, string imei)
        {
            this.IMEI = imei;
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorArgs"/> class.
        /// </summary>
        /// <param name="ex">
        /// The ex<see cref="Exception"/>.
        /// </param>
        /// <param name="imei">
        /// The IMEI.
        /// </param>
        public ErrorArgs(Exception ex, string imei)
        {
            this.IMEI = imei;
            this.Error = ex.Message;
        }

        /// <summary>
        /// Gets or sets the IMEI.
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the Error.
        /// </summary>
        /// <value>The error.</value>
        public string Error { get; set; }
    }
}
