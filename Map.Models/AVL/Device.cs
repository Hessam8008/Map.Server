// ***********************************************************************
// Assembly         : GPS.Models
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="Device.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.AVL
{
    using System;

    /// <summary>
    /// The device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the IMEI.
        /// </summary>
        /// <value>The IMEI of the device.</value>
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        /// <value>The nickname.</value>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the SIM number for device.
        /// </summary>
        /// <value>The SIM number.</value>
        public string SimNumber { get; set; }

        /// <summary>
        /// Gets or sets the model of the device.
        /// </summary>
        /// <value>The model.</value>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the device.
        /// </summary>
        /// <value>The serial number.</value>
        public string SN { get; set; }

        /// <summary>
        /// Gets or sets the owner mobile number.
        /// </summary>
        /// <value>The owner mobile number.</value>
        public string OwnerMobileNumber { get; set; }
    }
}