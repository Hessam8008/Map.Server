// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="CustomerRepo.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Map.DataAccess.DAO;
using System;

namespace Map.DataAccess.Repositories
{
    using Map.DataAccess.Dapper;
    using Map.Models.Customer;
    using Map.Models.Repositories;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Class DeviceRepo.
    /// Implements the <see cref="DapperRepository" />
    /// </summary>
    /// <seealso cref="DapperRepository" />
    internal class CustomerRepo : DapperRepository, ICustomerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepo"/> class. 
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        public CustomerRepo(IDbTransaction transaction)
            : base(transaction)
        {
        }

        /// <summary>Inserts the asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="ArgumentNullException">customer</exception>
        public async Task<int> InsertAsync(CustomerInfo customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var dao = new CustomerDao(customer);

            var param = dao.DynamicParameters();

            return await ExecuteAsync("[dbo].[stpCustomer_Insert]", param);
        }

        /// <summary>Updates the asynchronous.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="ArgumentNullException">customer</exception>
        public async Task<int> UpdateAsync(CustomerInfo customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var dao = new CustomerDao(customer);

            var param = dao.DynamicParameters();

            return await ExecuteAsync("[dbo].[stpCustomer_Update]", param);
        }

        /// <summary>Deletes the asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<int> DeleteAsync(int id)
        {
            var param = new { ID = id };

            return await ExecuteAsync("[dbo].[stpCustomer_Delete]", param);
        }

        /// <summary>Gets the by identifier asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CustomerInfo> GetByIdAsync(int id)
        {
            const string ProcedureName = "[dbo].[stpCustomer_GetById]";
            var param = new { id };
            var customer = await QueryFirstOrDefaultAsync<CustomerDao>(ProcedureName, param);
            return customer?.ToCustomerInfo();
        }

        /// <summary>
        /// The get a list of customers by area.
        /// </summary>
        /// <param name="area">
        /// The area.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> of enumerable <see cref="CustomerInfo"/>.
        /// </returns>
        public async Task<IEnumerable<CustomerInfo>> GetByAreaAsync(int area)
        {
            const string ProcedureName = "[dbo].[stpCustomer_GetByArea]";
            var param = new { area };
            return await QueryAsync<CustomerInfo>(ProcedureName, param);
        }
    }
}
