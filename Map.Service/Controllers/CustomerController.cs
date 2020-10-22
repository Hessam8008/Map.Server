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

using Microsoft.AspNetCore.Http;

namespace Map.Service.Controllers
{
    using Map.Models;
    using Map.Models.Customer;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The customer controller.
    /// </summary>
    [Produces("application/json")]
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

        /// <summary>Creates the asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([Required] CustomerInfo customer)
        {
            var result = await unitOfWork.CustomerRepository.InsertAsync(customer);

            if (result <= 0)
            {
                return NoContent();
            }

            unitOfWork.Commit();
            return Ok();
        }

        /// <summary>Updates the asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync([Required] CustomerInfo customer)
        {
            var result = await unitOfWork.CustomerRepository.UpdateAsync(customer);

            if (result <= 0)
            {
                return NoContent();
            }

            unitOfWork.Commit();
            return Ok();
        }

        /// <summary>Deletes the asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync([Required] int id)
        {
            var result = await unitOfWork.CustomerRepository.DeleteAsync(id);

            if (result <= 0)
            {
                return NoContent();
            }

            unitOfWork.Commit();
            return Ok();
        }

        /// <summary>Gets the by identifier asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [ProducesResponseType(typeof(CustomerInfo), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetAsync([Required] int id)
        {
            var result = await unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
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
        [HttpGet]
        [Route("GetByArea")]
        public async Task<IActionResult> GetByAreaAsync([Required] [FromQuery] int area = 1)
        {
            var result = await unitOfWork.CustomerRepository.GetByAreaAsync(area);
            if (result == null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
