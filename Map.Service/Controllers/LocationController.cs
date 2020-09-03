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
namespace Map.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Map.Models;
    using Map.Models.AVL;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("GetPath")]
        public async Task<IActionResult> GetPathAsync(
            [Required][FromQuery] int deviceId,
            [Required][FromQuery] DateTime from,
            [Required][FromQuery] DateTime to)
        {
            var result = await this.unitOfWork.LocationRepository.GetByDeviceAsync(deviceId, from, to);
            if (result == null || !result.Any())
            {
                return this.NoContent();
            }

            return this.Ok(result);
        }
    }
}
