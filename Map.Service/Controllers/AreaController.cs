using Map.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Map.Models.AVL;
using Microsoft.AspNetCore.Http;

namespace Map.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IMapUnitOfWork unitOfWork;
        public AreaController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>Gets all area's asynchronous.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [ProducesResponseType(typeof(IEnumerable<Area>), 200)]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await unitOfWork.AreaRepository.GetAllAsync();

            if (result == null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// The update location of the area async.
        /// </summary>
        /// <param name="id">
        /// The area Id.
        /// </param>
        /// <param name="latitude">
        /// The Latitude.
        /// </param>
        /// <param name="longitude">
        /// The Longitude.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}/UpdateLocation")]
        [HttpPut]
        public async Task<IActionResult> UpdateLocationAsync([Required][FromRoute] int id, float latitude, float longitude)
        {
            var result = await this.unitOfWork.AreaRepository.UpdateLocationAsync(id, latitude, longitude);
            this.unitOfWork.Commit();

            if (result > 0)
            {
                return this.Ok();
            }

            return this.NotFound();
        }
    }
}