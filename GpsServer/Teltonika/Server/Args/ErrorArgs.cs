namespace GpsServer.Teltonika.Server.Args
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
        /// <param name="error">The error<see cref="string"/>.</param>
        public ErrorArgs(string error)
        {
            Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorArgs"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public ErrorArgs(Exception ex)
        {
            Error = ex.Message;
        }

        /// <summary>
        /// Gets or sets the Error.
        /// </summary>
        public string Error { get; set; }
    }
}
