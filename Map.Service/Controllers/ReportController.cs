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
    public class ReportController : ControllerBase
    {

        private readonly IMapUnitOfWork unitOfWork;

        public ReportController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get last location of the devices.
        /// </summary>
        /// <param name="devices">List of devices ID.</param>
        /// <returns>Returns the list of points.</returns>
        /// <response code="200">Returns the list of AvlPackage.</response>
        /// <response code="204">If no data available.</response>            
        [HttpGet("GetLastLocations")]
        [ProducesResponseType(typeof(IEnumerable<Point>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLastLocationsAsync([Required][FromQuery] List<int> devices)
        {
            var result = await unitOfWork.ReportRepository.GetLastLocationsAsync(devices);
            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }
    }
}
