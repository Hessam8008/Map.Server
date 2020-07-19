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
    public class DeviceController : ControllerBase
    {
        private readonly IMapUnitOfWork unitOfWork;


        public DeviceController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllAsync()
        {
            var result = await unitOfWork.DeviceRepository.GetAllAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpGet("imei/{imei}")]
        public async Task<ActionResult<Device>> GetByIMEIAsync(string imei)
        {
            var result = await unitOfWork.DeviceRepository.GetByIMEIAsync(imei);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<Device>> GetByIdAsync(int id)
        {
            var result = await unitOfWork.DeviceRepository.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
