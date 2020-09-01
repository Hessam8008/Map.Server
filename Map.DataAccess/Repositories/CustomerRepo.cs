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
namespace Map.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using Map.DataAccess.Dapper;
    using Map.Models.Customer;
    using Map.Models.Repositories;

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
            return await this.QueryAsync<CustomerInfo>(ProcedureName, param);
        }
    }
}
