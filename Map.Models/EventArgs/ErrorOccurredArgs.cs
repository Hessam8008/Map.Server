// ***********************************************************************
// Assembly         : Map.Models
// Author           : Hessam Hosseini
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="ErrorOccurredArgs.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.EventArgs
{
    using System;

    /// <summary>
    /// Class ErrorOccurredArgs.
    /// Implements the <see cref="EventArgs" />
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class ErrorOccurredArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorOccurredArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ErrorOccurredArgs(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }
}