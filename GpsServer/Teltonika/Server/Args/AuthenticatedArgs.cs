namespace GpsServer.Teltonika.Server.Args
{
    public class AuthenticatedArgs
    {
        public string IMEI { get; }

        public bool Accepted { get; set; } = false;

        public AuthenticatedArgs(string imei)
        {
            IMEI = imei;
        }
    }
}