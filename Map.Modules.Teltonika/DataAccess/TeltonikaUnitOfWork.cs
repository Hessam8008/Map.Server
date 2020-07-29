// ***********************************************************************
// Assembly         : Map.Modules.Teltonika
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="TeltonikaUnitOfWork.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.DataAccess
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Map.Modules.Teltonika.DataAccess.Repositories;

    /// <summary>
    /// Class MapUnitOfWork.
    /// Implements the <see cref="IDisposable" />
    /// Implements the <see cref="ICloneable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="System.ICloneable" />
    internal class TeltonikaUnitOfWork : IDisposable, ICloneable
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
        /// The raw data repo
        /// </summary>
        private RawDataRepo rawDataRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeltonikaUnitOfWork" /> class.
        /// </summary>
        /// <param name="cs">The cs.</param>
        public TeltonikaUnitOfWork(string cs)
        {
            this.connectionString = cs;
            this.BeginTransaction();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TeltonikaUnitOfWork"/> class.
        /// </summary>
        ~TeltonikaUnitOfWork() => this.Dispose(false);

        /// <summary>
        /// Gets the raw data repository.
        /// </summary>
        /// <value>The raw data repository.</value>
        public RawDataRepo RawDataRepository =>
            this.rawDataRepo = this.rawDataRepo ?? new RawDataRepo(this.transaction);

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
        public object Clone() => (TeltonikaUnitOfWork)Activator.CreateInstance(this.GetType());

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
            this.rawDataRepo = null;
        }

        #endregion
    }
}
