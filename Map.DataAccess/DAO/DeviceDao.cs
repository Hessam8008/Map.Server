// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="DeviceDao.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess.DAO
{
    using System;

    using Map.DataAccess.Dapper;
    using Map.Models.AVL;

    /// <summary>
    /// Class Device.
    /// </summary>
    internal class DeviceDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDAO" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public DeviceDAO(Device device)
        {
            this.ID = device.ID;
            this.IMEI = device.IMEI;
            this.CreateTime = device.CreateTime;
            this.SimNumber = device.SimNumber;
            this.Model = device.Model;
            this.SN = device.SN;
            this.OwnerMobileNumber = device.OwnerMobileNumber;
            this.Nickname = device.Nickname;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the IMEI.
        /// </summary>
        /// <value>The IMEI.</value>
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        [DapperIgnoreParameter]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        /// <value>The nickname.</value>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>The mobile number.</value>
        public string SimNumber { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>The serial number.</value>
        public string SN { get; set; }

        /// <summary>
        /// Gets or sets the owner mobile number.
        /// </summary>
        /// <value>The owner mobile number.</value>
        public string OwnerMobileNumber { get; set; }

        /// <summary>
        /// Converts to device.
        /// </summary>
        /// <returns>returns the <exception cref="Device">Device</exception>.</returns>
        public Device ToDevice()
        {
            var device = new Device
            {
                OwnerMobileNumber = this.OwnerMobileNumber,
                SimNumber = this.SimNumber,
                CreateTime = this.CreateTime,
                ID = this.ID,
                IMEI = this.IMEI,
                Model = this.Model,
                Nickname = this.Nickname,
                SN = this.SN
            };
            return device;
        }
    }
}
