// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 09-01-2020
//
// Last Modified By : U12178
// Last Modified On : 09-01-2020
// ***********************************************************************
// <copyright file="CustomerController.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Service.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Map.Models;
    using Map.Models.Customer;

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
        /// Get all customers registered in the system by area.
        /// </summary>
        /// <param name="area">
        /// The area id.
        /// </param>
        /// <returns>
        /// List of customers
        /// </returns>
        /// <response code="200">Returns a list of customers.</response>
        /// <response code="204">If no customer found.</response>
        [ProducesResponseType(typeof(IEnumerable<CustomerInfo>), 200)]
        [HttpGet("GetByArea")]
        public async Task<IActionResult> GetByAreaAsync([Required] [FromQuery] int area = 1)
        {
            var result = await this.unitOfWork.CustomerRepository.GetByAreaAsync(area);
            if (result == null || !result.Any())
            {
                return this.NoContent();
            }

            return this.Ok(result);
        }
    }
}
