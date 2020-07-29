// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 06-18-2020
//
// Last Modified By : U12178
// Last Modified On : 06-18-2020
// ***********************************************************************
// <copyright file="DapperRepository.cs" company="Golriz">
//     Copyright (c) 2020 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Modules.Teltonika.DataAccess.Dapper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using global::Dapper;

    /// <summary>
    /// Class DapperRepository.
    /// </summary>
    public abstract class DapperRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperRepository"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        protected DapperRepository(IDbTransaction transaction) => this.Transaction = transaction;

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        protected IDbTransaction Transaction { get; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        protected IDbConnection Connection => this.Transaction?.Connection;

        /// <summary>
        /// Queries the specified procedure name.
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>IEnumerable of T</returns>
        protected IEnumerable<T> Query<T>(string procedureName, object param = null)
        {
            var entities = this.Connection.Query<T>(
                sql: procedureName,
                param: param,
                commandType: CommandType.StoredProcedure,
                transaction: this.Transaction);

            return entities;
        }

        /// <summary>
        /// Queries the multiple.
        /// </summary>
        /// <param name="sqlScript">The SQL query.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Return a GridReader.</returns>
        protected SqlMapper.GridReader QueryMultiple(string sqlScript, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var grid = this.Connection.QueryMultiple(
                sql: sqlScript,
                param: param,
                commandType: commandType,
                transaction: this.Transaction);
            
            return grid;
        }

        /// <summary>
        /// Queries the first or default.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Object of T.</returns>
        protected T QueryFirstOrDefault<T>(string procedureName, object param = null)
        {
            var entity = this.Connection.QueryFirstOrDefault<T>(
                sql: procedureName,
                param: param,
                        commandType: CommandType.StoredProcedure,
                        transaction: this.Transaction);
            return entity;
        }

        /// <summary>
        /// Queries the single or default.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Object of T.</returns>
        protected T QuerySingleOrDefault<T>(string procedureName, object param = null)
        {
            var entity =
                this.Connection.QuerySingleOrDefault<T>(
                        sql: procedureName,
                        param: param,
                        commandType: CommandType.StoredProcedure,
                        transaction: this.Transaction);

            return entity;
        }

        /// <summary>
        /// Execute a stored procedure and return rows affected.
        /// </summary>
        /// <param name="procedureName">Stored procedure name</param>
        /// <param name="param">Stored procedure parameters</param>
        /// <returns>Integer as affected rows</returns>
        /// <remarks>Must call 'Commit()' at the end to commit changes to the database.</remarks>
        protected int Execute(string procedureName, object param = null)
        {
            return this.Connection.Execute(
                sql: procedureName,
                param: param,
                commandType: CommandType.StoredProcedure,
                transaction: this.Transaction);
        }

        /// <summary>
        /// Execute query and return first cell of first row
        /// </summary>
        /// <typeparam name="T">Type of result</typeparam>
        /// <param name="procedureName">Stored procedure name</param>
        /// <param name="param">Stored procedure parameters</param>
        /// <returns>First cell of first row</returns>
        protected T ExecuteScalar<T>(string procedureName, object param = null)
        where T : struct
        {
            var result =
                this.Connection.ExecuteScalar<T>(
                    sql: procedureName,
                        param: param,
                        commandType: CommandType.StoredProcedure,
                        transaction: this.Transaction);
            return result;
        }

        /// <summary>
        /// Query as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>IEnumerable Task of T.</returns>
        protected async Task<IEnumerable<T>> QueryAsync<T>(string procedureName, object param = null)
        {
            var entities = await
                this.Connection.QueryAsync<T>(
                        sql: procedureName,
                        param: param,
                        commandType: CommandType.StoredProcedure,
                        transaction: this.Transaction);

            return entities;
        }

        /// <summary>
        /// Query multiple as an asynchronous operation.
        /// </summary>
        /// <param name="sql">The SQL query.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Task of GridReader.</returns>
        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return await this.Connection.QueryMultipleAsync(
                    sql: sql,
                    param: param,
                    commandType: commandType,
                    transaction: this.Transaction);
        }

        /// <summary>
        /// query first or default as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Task of T.</returns>
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string procedureName, object param = null)
        {
            var entity = await
                             this.Connection.QueryFirstOrDefaultAsync<T>(
                                 sql: procedureName,
                                 param: param,
                                 commandType: CommandType.StoredProcedure,
                                 transaction: this.Transaction);
            return entity;
        }

        /// <summary>
        /// query single or default as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Task of T.</returns>
        protected async Task<T> QuerySingleOrDefaultAsync<T>(string procedureName, object param = null)
        {
            var entity = await
                             this.Connection.QuerySingleOrDefaultAsync<T>(
                                 sql: procedureName,
                                 param: param,
                                 commandType: CommandType.StoredProcedure,
                                 transaction: this.Transaction);
            return entity;
        }

        /// <summary>
        /// Execute as an asynchronous operation.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Task of integer.</returns>
        protected async Task<int> ExecuteAsync(string procedureName, object param = null)
        {
            var result = await
                this.Connection.ExecuteAsync(
                        sql: procedureName,
                        param: param,
                        commandType: CommandType.StoredProcedure,
                        transaction: this.Transaction);

            return result;
        }

        /// <summary>
        /// execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Task of T.</returns>
        protected async Task<T> ExecuteScalarAsync<T>(string procedureName, object param = null)
        where T : struct
        {
            var result = await
                             this.Connection.ExecuteScalarAsync<T>(
                                 sql: procedureName,
                                 param: param,
                                 commandType: CommandType.StoredProcedure,
                                 transaction: this.Transaction);
            return result;
        }
    }
}