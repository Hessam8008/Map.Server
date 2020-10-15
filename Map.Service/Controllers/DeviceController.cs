// ***********************************************************************
// Assembly         : Map.Service
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="DeviceController.cs" company="Map.Service">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Service.Controllers
{
    using Map.Models;
    using Map.Models.AVL;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Class DeviceController.
    /// Implements the <see cref="ControllerBase" />
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IMapUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeviceController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(Device), 200)]
        public async Task<IActionResult> CreateAsync([Required] Device device)
        {
            var result = await unitOfWork.DeviceRepository.InsertAsync(device);

            if (result == 0)
            {
                return NoContent();
            }

            var insertedDevice = await unitOfWork.DeviceRepository.GetByIdAsync(result);

            unitOfWork.Commit();
            return Ok(insertedDevice);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateAsync([Required] Device device)
        {
            var result = await unitOfWork.DeviceRepository.UpdateAsync(device);

            if (result <= 0)
            {
                return NoContent();
            }

            unitOfWork.Commit();
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync([Required] int id)
        {
            var result = await unitOfWork.DeviceRepository.DeleteAsync(id);

            if (result <= 0)
            {
                return NoContent();
            }

            unitOfWork.Commit();
            return Ok();
        }

        /// <summary>
        /// Get all devices registered in the system.
        /// </summary>
        /// <returns>List of devices</returns>
        /// <response code="200">Returns a list of devices.</response>
        /// <response code="204">If no device found.</response>
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await unitOfWork.DeviceRepository.GetAllAsync();
            if (result == null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get device by ID.
        /// </summary>
        /// <param name="id">ID of the device.</param>
        /// <returns>The <see cref="Device"/></returns>
        /// <response code="200">Returns the device.</response>
        /// <response code="204">If no device found.</response>
        [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([Required] int id)
        {
            var result = await unitOfWork.DeviceRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get device by IMEI.
        /// </summary>
        /// <param name="imei">IMEI of the device.</param>
        /// <returns>The <see cref="Device"/></returns>
        /// <response code="200">Returns the device.</response>
        /// <response code="204">If no device found.</response>
        [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
        [HttpGet("GetByIMEI")]
        public async Task<IActionResult> GetByIMEIAsync([Required][FromQuery] string imei)
        {
            var result = await unitOfWork.DeviceRepository.GetByIMEIAsync(imei);
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
