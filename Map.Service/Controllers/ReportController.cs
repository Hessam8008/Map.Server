﻿// ***********************************************************************
// Assembly         : Map.Service
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="ReportController.cs" company="Golriz">
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
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using Map.Models;
    using Map.Models.AVL;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Class ReportController.
    /// Implements the <see cref="ControllerBase" />
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IMapUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReportController(IMapUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get last location of the devices.
        /// </summary>
        /// <param name="devices">List of devices ID.</param>
        /// <returns>Returns the list of points.</returns>
        /// <response code="200">Returns the list of points.</response>
        /// <response code="204">If no data available.</response>
        [HttpGet("GetLastLocations")]
        [ProducesResponseType(typeof(IEnumerable<Point>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLastLocationsAsync([Optional][FromQuery] List<int> devices)
        {
            var result = await this.unitOfWork.ReportRepository.GetLastLocationsAsync(devices);
            if (result == null || !result.Any())
            {
                return this.NoContent();
            }

            return this.Ok(result);
        }
    }
}
