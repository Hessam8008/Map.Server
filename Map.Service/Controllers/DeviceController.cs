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
        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            var result = await unitOfWork.DeviceRepository.GetAllAsync();
            return result;
        }
        
        [HttpGet]
        public async Task<Device> GetByIMEIAsync(string imei)
        {
            var result = await unitOfWork.DeviceRepository.GetByIMEIAsync(imei);
            return result;
        }
        
        [HttpGet]
        public async Task<Device> GetByIdAsync(int id)
        {
            var result = await unitOfWork.DeviceRepository.GetByIdAsync(id);
            return result;
        }

    }
}
