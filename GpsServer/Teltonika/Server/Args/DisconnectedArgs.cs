namespace GpsServer.Teltonika.Server.Args
{
    /// <summary>
    /// The disconnected args.
    /// </summary>
    public class DisconnectedArgs
    {
        /// <summary>
        /// Gets the IMEI.
        /// </summary>
        public string IMEI { get; }

        public DisconnectedArgs(string imei)
        {
            this.IMEI = imei;
        }
    }
}