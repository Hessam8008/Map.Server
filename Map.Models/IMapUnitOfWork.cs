// ***********************************************************************
// Assembly         : Map.Models
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="IMapUnitOfWork.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Models
{
    using Map.Models.Repositories;

    /// <summary>
    /// Interface IMapUnitOfWork
    /// </summary>
    public interface IMapUnitOfWork
    {
        /// <summary>
        /// Gets the device repository.
        /// </summary>
        /// <value>The device repository.</value>
        public IDeviceRepository DeviceRepository { get; }

        /// <summary>
        /// Gets the location repository.
        /// </summary>
        /// <value>The location repository.</value>
        public ILocationRepository LocationRepository { get; }

        /// <summary>
        /// Gets the report repository.
        /// </summary>
        /// <value>The report repository.</value>
        public IReportRepository ReportRepository { get; }

        /// <summary>
        /// Gets the customer repository.
        /// </summary>
        /// <value>The customer repository.</value>
        public ICustomerRepository CustomerRepository { get; }

        /// <summary>Gets the area repository.</summary>
        /// <value>The area repository.</value>
        public IAreaRepository AreaRepository { get; }

        /// <summary>Commits this instance.</summary>
        public void Commit();
    }
}
