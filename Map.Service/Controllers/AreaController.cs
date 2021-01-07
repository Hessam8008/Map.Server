using Map.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Map.Models.AVL;

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

        /// <summary>Gets all asynchronous.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [ProducesResponseType(typeof(IEnumerable<Area>), 200)]
        [Route("GetAll")]
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
        /// The area id.
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
        [ProducesResponseType(typeof(IEnumerable<Area>), 200)]
        [Route("UpdateLocation")]
        [HttpGet]
        public async Task<IActionResult> UpdateLocationAsync(int id, float latitude, float longitude)
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