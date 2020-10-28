// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="MapUnitOfWork.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Map.Models;
    using Map.Models.Repositories;

    using Repositories;

    /// <summary>
    /// Class MapUnitOfWork.
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class MapUnitOfWork : IDisposable, ICloneable, IMapUnitOfWork
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The transaction
        /// </summary>
        private IDbTransaction transaction;

        /// <summary>
        /// The SQL connection.
        /// </summary>
        private IDbConnection connection;
        
        /// <summary>
        /// The device repository.
        /// </summary>
        private IDeviceRepository deviceRepo;

        /// <summary>
        /// The location repo
        /// </summary>
        private ILocationRepository locationRepo;

        /// <summary>
        /// The report repo
        /// </summary>
        private IReportRepository reportRepo;

        /// <summary>
        /// The customer repo.
        /// </summary>
        private ICustomerRepository customerRepo;

        /// <summary>The area repository</summary>
        private IAreaRepository areaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapUnitOfWork" /> class.
        /// </summary>
        /// <param name="dataSettings">The database settings.</param>
        public MapUnitOfWork(IDatabaseSettings dataSettings)
        {
            this.connectionString = dataSettings.ConnectionString;
            this.BeginTransaction();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MapUnitOfWork"/> class.
        /// </summary>
        ~MapUnitOfWork() => this.Dispose(false);

        /// <summary>
        /// Gets the device repository.
        /// </summary>
        /// <value>The device repository.</value>
        public IDeviceRepository DeviceRepository =>
            this.deviceRepo = this.deviceRepo ?? new DeviceRepo(this.transaction);

        /// <summary>
        /// Gets the location repository.
        /// </summary>
        /// <value>The location repository.</value>
        public ILocationRepository LocationRepository =>
            this.locationRepo = this.locationRepo ?? new LocationRepo(this.transaction);

        /// <summary>
        /// Gets the report repository.
        /// </summary>
        /// <value>The report repository.</value>
        public IReportRepository ReportRepository =>
            this.reportRepo = this.reportRepo ?? new ReportRepo(this.transaction);

        /// <summary>
        /// The customer repository.
        /// </summary>
        public ICustomerRepository CustomerRepository =>
            this.customerRepo = this.customerRepo ?? new CustomerRepo(this.transaction);

        /// <summary>Gets the area repository.</summary>
        /// <value>The area repository.</value>
        public IAreaRepository AreaRepository =>
            this.areaRepository = this.areaRepository ?? new AreaRepository(this.transaction);

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            /*~~~~~~~~~~~~~~~ SQL SERVER ~~~~~~~~~~~~~~~*/
            /* Dispose */
            this.transaction?.Rollback();
            this.transaction?.Dispose();
            this.connection?.Close();
            this.connection?.Dispose();
            /* Create */
            this.connection = new SqlConnection(this.connectionString);
            this.connection.Open();
            this.transaction = this.connection.BeginTransaction();
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void Commit()
        {
            try
            {
                this.transaction.Commit();
            }
            catch (Exception)
            {
                this.transaction.Rollback();
                throw;
            }
            finally
            {
                this.Dispose();
            }

            this.BeginTransaction();
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void Rollback()
        {
            try
            {
                this.transaction.Rollback();
            }
            finally
            {
                this.Dispose();
            }

            this.BeginTransaction();
        }

        #region ►| IDisposable |◄
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() => (MapUnitOfWork)Activator.CreateInstance(this.GetType());

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            // Sql
            this.transaction?.Dispose();
            this.transaction = null;
            this.connection?.Close();
            this.connection?.Dispose();
            this.connection = null;

            // Dispose all repositories.
            this.deviceRepo = null;
            this.locationRepo = null;
            this.reportRepo = null;
            this.customerRepo = null;
            this.areaRepository = null;
        }
        
        #endregion
    }
}
