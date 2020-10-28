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

    }
}