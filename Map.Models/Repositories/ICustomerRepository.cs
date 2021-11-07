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
    using Map.Models.Customer;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface IReportRepository
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>Inserts the asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>
        ///   Return 1 if new customer added to the table. 
        /// </returns>
        Task<int> InsertAsync(CustomerInfo customer);

        /// <summary>Updates the asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>
        ///    Count of affected rows
        /// </returns>
        Task<int> UpdateAsync(CustomerInfo customer);

        /// <summary>Deletes the asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   Count of affected rows
        /// </returns>
        Task<int> DeleteAsync(int id);

        /// <summary>Gets the by identifier asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   CustomerInfo
        /// </returns>
        Task<CustomerInfo> GetByIdAsync(int id);

        /// <summary>
        /// Gets the customers asynchronous.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <returns>IEnumerable CustomerInfo.</returns>
        Task<IEnumerable<CustomerInfo>> GetByAreaAsync(int area);

        /// <summary>
        /// Synchronizes the ACE changes asynchronous.
        /// </summary>
        Task SyncChangesAsync();
    }
}