using System;
using System.Collections.Generic;
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


        [HttpGet("{deviceId:int}/{from}/{to}")]
        public async Task<ActionResult<IEnumerable<Location>>> Get(int deviceId, DateTime from, DateTime to)
        {
            var result = await unitOfWork.LocationRepository.GetByDeviceAsync(deviceId, from, to);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
