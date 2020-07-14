using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Map.DataAccess;
using Map.Models.AVL;
using Map.Modules.Teltonika;

namespace Map.Server
{
    class TeltonikaBlackBox : IBlackBox
    {
        public string ConnectionString { get; }

        public TeltonikaBlackBox()
        {
#if DEBUG
            ConnectionString = "server=dm1server1;uid=dbUser;pwd=1234;database=GPS;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#else
            ConnectionString = "server=10.10.1.12\\GCAS;database=GPSTrackerDB;uid=DVP1;pwd=Fly#3592;MultipleActiveResultSets=True;Application Name=MAP.SERVER";
#endif
        }

        public async Task<bool> ApprovedIMEIAsync(string imei)
        {
            using var uow = new MapUnitOfWork(ConnectionString);
            var device = await uow.DeviceRepository.GetByIMEIAsync(imei).ConfigureAwait(false);
            if (device != null)
                return true;

            device = new Device
            {
                IMEI = imei,
                Model = "N/A",
                SimNumber = "0000000000",
                OwnerMobileNumber = "0000000000",
                Nickname = "N/A",
                SN = "N/A"
            };
            await uow.DeviceRepository.SyncAsync(device).ConfigureAwait(false);
            uow.Commit();
            Console.WriteLine($"BB: ApprovedIMEIAsync {imei}");
            return true;
        }

        public async Task<bool> AcceptedLocationsAsync(string imei, List<Location> locations)
        {
            using var db = new MapUnitOfWork(ConnectionString);
            var device = await db.DeviceRepository.GetByIMEIAsync(imei).ConfigureAwait(false);
            if (device == null)
            {
                return false;
            }
            foreach (var location in locations)
            {
                location.Device = device;
                var locationId = await db.LocationRepository.Insert(location).ConfigureAwait(false);
            }
            db.Commit();
            Console.WriteLine($"BB: AcceptedLocationsAsync {imei}, count: {locations.Count}");

            return true;
        }
    }
}
