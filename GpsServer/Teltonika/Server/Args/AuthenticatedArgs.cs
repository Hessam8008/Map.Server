namespace GpsServer.Teltonika.Server.Args
{
    /// <summary>
    /// Defines the <see cref="AuthenticatedArgs" />.
    /// </summary>
    public class AuthenticatedArgs
    {
        /// <summary>
        /// Gets the IMEI.
        /// </summary>
        public string IMEI { get; }

        /// <summary>
        /// Gets or sets a value indicating whether Accepted.
        /// </summary>
        public bool Accepted { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedArgs"/> class.
        /// </summary>
        /// <param name="imei">The imei<see cref="string"/>.</param>
        public AuthenticatedArgs(string imei)
        {
            IMEI = imei;
        }
    }
}
