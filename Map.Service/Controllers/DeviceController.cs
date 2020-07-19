using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Map.Models;
using Map.Models.AVL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Map.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IMapUnitOfWork unitOfWork;


        public DeviceController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
                return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// Get device by IMEI.
        /// </summary>
        /// <param name="imei">IMEI of the device.</param>
        /// <returns>Device</returns>
        /// <response code="200">Returns the device.</response>
        /// <response code="204">If no device found.</response>   
        [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
        [HttpGet("imei/{imei}")]
        public async Task<IActionResult> GetByIMEIAsync([Required] string imei)
        {
            var result = await unitOfWork.DeviceRepository.GetByIMEIAsync(imei);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// Get device by ID.
        /// </summary>
        /// <param name="id">ID of the device.</param>
        /// <returns>Device</returns>
        /// <response code="200">Returns the device.</response>
        /// <response code="204">If no device found.</response>   
        [ProducesResponseType(typeof(Device),StatusCodes.Status200OK)]
        [HttpGet("id/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([Required] int id)
        {
            var result = await unitOfWork.DeviceRepository.GetByIdAsync(id);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

    }
}
