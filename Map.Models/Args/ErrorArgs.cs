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

using System;

namespace Map.Models.Args
{
    /// <summary>
    /// Defines the <see cref="ErrorArgs" />.
    /// </summary>
    public class ErrorArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorArgs"/> class.
        /// </summary>
        /// <param name="message"></param>
        public ErrorArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorArgs"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"> The ex<see cref="Exception"/>. </param>

        public ErrorArgs(string message, Exception ex)
        {
            this.Message = message;
            Exception = ex;
        }

        public ErrorArgs(Exception ex)
        {
            Exception = ex;
        }

        /// <summary>
        /// Gets or sets the Error.
        /// </summary>
        /// <value>The error.</value>
        public string Message { get; }


        public Exception Exception { get; }

    }
}
