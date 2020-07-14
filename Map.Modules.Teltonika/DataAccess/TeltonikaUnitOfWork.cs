// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="MapDb.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using Map.Modules.Teltonika.DataAccess.Repositories;


namespace Map.Modules.Teltonika.DataAccess
{
    /// <summary>
    /// Class MapUnitOfWork.
    /// Implements the <see cref="System.IDisposable" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="System.ICloneable" />
    internal class TeltonikaUnitOfWork : IDisposable, ICloneable
    {
        /// <summary>
        /// The transaction
        /// </summary>
        private IDbTransaction transaction;

        /// <summary>
        /// The SQL connection.
        /// </summary>
        private IDbConnection connection;

        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        
        private RawDataRepo rawDataRepo;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="TeltonikaUnitOfWork"/> class.
        /// </summary>
        /// <param name="cs">The cs.</param>
        public TeltonikaUnitOfWork(string cs)
        {
            this.connectionString = cs;
            this.BeginTransaction();
        }

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
                Dispose();
            }
            BeginTransaction();
        }

        public RawDataRepo RawDataRepository => this.rawDataRepo ??= new RawDataRepo(this.transaction);
        
        #region ►| IDisposable |◄

        ~TeltonikaUnitOfWork() => this.dispose(false);

        public object Clone() => (TeltonikaUnitOfWork)Activator.CreateInstance(this.GetType());

        private void dispose(bool disposing)
        {
            if (!disposing) 
                return;

            //Sql
            this.transaction?.Dispose();
            this.transaction = null;
            this.connection?.Close();
            this.connection?.Dispose();
            this.connection = null;


            //Dispose all repositories.
            this.rawDataRepo = null;
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
