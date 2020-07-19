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
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly IMapUnitOfWork unitOfWork;
        
        public LocationController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns the list of a device location in a period.
        /// </summary>
        /// <param name="deviceId">ID of the device</param>
        /// <param name="from">Start time</param>
        /// <param name="to">End time</param>
        /// <returns>List of locations.</returns>
        /// <response code="200">Returns a list of locations.</response>
        /// <response code="204">If no location found.</response>   
        [ProducesResponseType(typeof(IEnumerable<Location>), StatusCodes.Status200OK)]
        [HttpGet("{deviceId:int}/{from}/{to}")]
        public async Task<IActionResult> Get
        (
            [Required]int deviceId, 
            [Required]DateTime from, 
            [Required]DateTime to
            )
        {
            var result = await unitOfWork.LocationRepository.GetByDeviceAsync(deviceId, from, to);
            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }
    }
}
