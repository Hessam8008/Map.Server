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
        public Task<IEnumerable<CustomerInfo>> GetCustomerByArea(int area);
    }
}