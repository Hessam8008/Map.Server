using Map.Models;

namespace Map.Server
{
    class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; }

        public DatabaseSettings()
        {
#if DEBUG
            ConnectionString = "server=dm1server1;uid=dbUser;pwd=1234;database=GPS;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#else
            ConnectionString = "server=10.10.1.12\\GCAS;database=GPSTrackerDB;uid=DVP1;pwd=Fly#3592;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#endif

        }
    }
}