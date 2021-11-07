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
    using Microsoft.AspNetCore.Http;
    using Models;
    using Models.Customer;
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

        /// <summary>Create a customer asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        [HttpPost]
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

        /// <summary>Updates the customer asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        [HttpPut]
        //[Route("Update")]
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

        /// <summary>Deletes a customer asynchronous.</summary>
        /// <param name="id">The customer identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpDelete]
        //[Route("Delete")]
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

        /// <summary>Gets the customer by identifier asynchronous.</summary>
        /// <param name="id">The customer identifier.</param>
        [ProducesResponseType(typeof(CustomerInfo), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync([Required][FromRoute] int id)
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
        [Route("area/{area}")]
        public async Task<IActionResult> GetByAreaAsync([Required][FromRoute] int area = 1)
        {
            var result = await unitOfWork.CustomerRepository.GetByAreaAsync(area);
            if (result == null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Sync all customers registered in the ACE database.
        /// </summary>
        /// <response code="200">Success</response>
        [HttpPut]
        [Route("SyncChanges")]
        public async Task<IActionResult> GetChangesAsync()
        {
            await unitOfWork.CustomerRepository.SyncChangesAsync();
            return Ok();
        }
    }
}
