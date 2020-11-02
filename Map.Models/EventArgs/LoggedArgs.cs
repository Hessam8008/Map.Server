// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="LoggedArgs.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.EventArgs
{
    using System;

    /// <summary>
    /// Class LoggedArgs.
    /// Implements the <see cref="EventArgs" />
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class LoggedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LoggedArgs(string message)
        {
            this.Message = message;
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; }
    }
}