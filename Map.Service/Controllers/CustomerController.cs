namespace Map.Service.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Map.Models;
    using Map.Models.AVL;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The customer controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IMapUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CustomerController(IMapUnitOfWork unitOfWork)
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
            var result = await this.unitOfWork.DeviceRepository.GetAllAsync();
            if (result == null || !result.Any())
            {
                return this.NoContent();
            }

            return this.Ok(result);
        }

    }
}
