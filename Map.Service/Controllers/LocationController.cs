// ***********************************************************************
// Assembly         : Map.Service
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="LocationController.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Map.Service.Requests.LocationAgg;

namespace Map.Service.Controllers
{
    using Map.Models;
    using Map.Models.AVL;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Class LocationController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IMapUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public LocationController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody, Required] AddLocation location)
        {
            var result = await unitOfWork.LocationRepository.InsertAsync(location.DeviceID, location.ToLocation());

            if (result == 0)
            {
                return NoContent();
            }

            unitOfWork.Commit();
            return Ok();
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
        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocationsAsync(
            [Required][FromQuery] int deviceId,
            [Required][FromQuery] DateTime from,
            [Required][FromQuery] DateTime to)
        {
            var result = await unitOfWork.LocationRepository.GetByDeviceAsync(deviceId, from, to);
            if (result == null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
