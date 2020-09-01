// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 09-01-2020
//
// Last Modified By : U12178
// Last Modified On : 09-01-2020
// ***********************************************************************
// <copyright file="ICustomerRepository.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.Models.Customer;

    /// <summary>
    /// Interface IReportRepository
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Gets the customers asynchronous.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <returns>Task of IEnumerable CustomerInfo.</returns>
        public Task<IEnumerable<CustomerInfo>> GetByAreaAsync(int area);
    }
}