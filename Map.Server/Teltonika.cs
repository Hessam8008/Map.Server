using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Map.DataAccess;
using Map.Models;
using Map.Models.AVL;
using Map.Modules.Teltonika;

namespace Map.Server
{
    class TeltonikaBlackBox : IBlackBox
    {
        public IDatabaseSettings dbSettings { get; }

        public TeltonikaBlackBox(IDatabaseSettings settings)
        {
            dbSettings = settings;
        }

        public async Task<bool> ApprovedIMEIAsync(string imei)
        {
            using var uow = new MapUnitOfWork(dbSettings);
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
            //Console.WriteLine($"BB: ApprovedIMEIAsync {imei}");
            return true;
        }

        public async Task<bool> AcceptedLocationsAsync(string imei, List<Location> locations)
        {
            using var db = new MapUnitOfWork(dbSettings);
            var device = await db.DeviceRepository.GetByIMEIAsync(imei).ConfigureAwait(false);
            if (device == null)
            {
                return false;
            }
            foreach (var location in locations)
            {
                location.Device = device;
                var locationId = await db.LocationRepository.InsertAsync(location).ConfigureAwait(false);
            }
            db.Commit();
            //Console.WriteLine($"BB: AcceptedLocationsAsync {imei}, count: {locations.Count}");

            return true;
        }
    }
}
