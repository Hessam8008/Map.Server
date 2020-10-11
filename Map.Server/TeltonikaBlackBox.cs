// ***********************************************************************
// Assembly         : Map.Server
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="TeltonikaBlackBox.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Server
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Map.DataAccess;
    using Map.Models;
    using Map.Models.AVL;
    using Map.Modules.Teltonika;

    /// <summary>
    /// Class Teltonika Black Box.
    /// Implements the <see cref="IBlackBox" />
    /// </summary>
    /// <seealso cref="IBlackBox" />
    internal class TeltonikaBlackBox : IBlackBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeltonikaBlackBox"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public TeltonikaBlackBox(IDatabaseSettings settings)
        {
            this.DatabaseSettings = settings;
        }

        /// <summary>
        /// Gets the database settings.
        /// </summary>
        /// <value>The database settings.</value>
        public IDatabaseSettings DatabaseSettings { get; }

        /// <summary>
        /// approved IMEI as an asynchronous operation.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <returns>The task of boolean.</returns>
        public async Task<bool> ApprovedIMEIAsync(string imei)
        {
            try
            {
                MapUnitOfWork uow;
                using (uow = new MapUnitOfWork(this.DatabaseSettings))
                {
                    var device = await uow.DeviceRepository.GetByIMEIAsync(imei).ConfigureAwait(false);
                    if (device != null)
                    {
                        return true;
                    }

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
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
           
        }

        /// <summary>
        /// accepted locations asynchronously.
        /// </summary>
        /// <param name="imei">The IMEI.</param>
        /// <param name="locations">The locations.</param>
        /// <returns>The task of boolean.</returns>
        public async Task<bool> AcceptedLocationsAsync(string imei, List<Location> locations)
        {
            try
            {
                MapUnitOfWork db;
                using (db = new MapUnitOfWork(this.DatabaseSettings))
                {
                    var device = await db.DeviceRepository.GetByIMEIAsync(imei).ConfigureAwait(false);
                    if (device == null)
                    {
                        return false;
                    }

                    foreach (var location in locations)
                    {
                        var locationId = await db.LocationRepository.InsertAsync(device.ID, location).ConfigureAwait(false);
                    }

                    db.Commit();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
           
        }
    }
}
