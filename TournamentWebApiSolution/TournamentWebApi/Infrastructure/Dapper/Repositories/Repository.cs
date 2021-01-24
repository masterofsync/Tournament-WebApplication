using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class Repository : IDisposable, IRepository
    {
        public IConfiguration Configuration { get; set; }
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public IDbConnection GetConnection => _connection;

        public IDbTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        protected Repository(IConfiguration config)
        {
            Configuration = config;
        }

        /// <summary>
        /// Get connection string from secret and return sql connection
        /// </summary>
        /// <returns>SqlConnection type</returns>
        private SqlConnection SqlConnection()
        {
            //return new SqlConnection(Configuration["AzureTournamentDBCOnnection"]);
            return new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
        }

        /// <summary>
        /// Open the connection
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }


        /// <summary>
        /// Open connection and begin transaction of that connection.
        /// </summary>
        public void StartTransaction()
        {
            _connection = SqlConnection();
            _connection.Open();

            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        /// <summary>
        /// Load data in transaction single or multiple. 
        /// </summary>
        /// <typeparam name="T">model Type </typeparam>
        /// <typeparam name="U">dynamic?</typeparam>
        /// <param name="sqlQuery">Sql query as String</param>
        /// <param name="parameters">parameters related to the sql query</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> LoadDataInTransactionUsingQueryAsync<T, U>(string sqlQuery, U parameters)
        {
            try
            {
                var rows = await _connection.QueryAsync<T>(sqlQuery, parameters,
                                  commandType: CommandType.Text, transaction: _transaction);
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load single data in transaction
        /// </summary>
        /// <typeparam name="T">model Type</typeparam>
        /// <typeparam name="U">dynamic?</typeparam>
        /// <param name="sqlQuery">Sql query as String</param>
        /// <param name="parameters">parameters related to the sql query</param>
        /// <returns></returns>
        public async Task<T> LoadSingleDataInTransactionUsingQueryAsync<T, U>(string sqlQuery, U parameters)
        {
            try
            {
                var row = await _connection.QuerySingleOrDefaultAsync<T>(sqlQuery, parameters,
                                  commandType: CommandType.Text, transaction: _transaction);
                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save data to the transaction
        /// </summary>
        /// <typeparam name="T">Parameters type</typeparam>
        /// <param name="sqlQuery">Sql query as String</param>
        /// <param name="parameters">parameters related to the sql query</param>
        /// <returns>Int: if greater than 0, sucessfully saved/updated</returns>
        public async Task<int> SaveDataInTransactionUsingQueryAsync<T>(string sqlQuery, T parameters)
        {
            try
            {
                return await _connection.ExecuteAsync(sqlQuery, parameters,
                                   commandType: CommandType.Text, transaction: _transaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save data to the transaction and get Id.
        /// </summary>
        /// <typeparam name="T">Parameters type</typeparam>
        /// <param name="sqlQuery">Sql query as String</param>
        /// <param name="parameters">parameters related to the sql query</param>
        /// <returns>Int: if greater than 0, sucessfully saved/updated</returns>
        public async Task<int> SaveDataInTransactionAndGetIdAsync<T>(string sqlQuery, T parameters)
        {
            try
            {
                return await _connection.QuerySingleOrDefaultAsync<int>(sqlQuery, parameters,
                                                commandType: CommandType.Text, transaction: _transaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private bool isClosed = false;

        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
                _connection?.Close();

                isClosed = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
                _connection?.Close();
                isClosed = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    // Log??
                }
            }
            _transaction = null;
            _connection = null;
        }


        #region Quick way to get Data without transaction
        /// <summary>
        /// Load data async. Get data from the database.
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <typeparam name="U">parameters data type</typeparam>
        /// <param name="query">sql query as string</param>
        /// <param name="parameters">parameters related to the sql query</param>
        /// <returns>data of the class type</returns>
        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string query, U parameters)
        {
            using (IDbConnection conn = SqlConnection())
            {
                var rows = await conn.QueryAsync<T>(query, parameters,
                                commandType: CommandType.Text);

                return rows;
            }
        }

        /// <summary>
        /// Save data async. Save data to the database.
        /// </summary>
        /// <typeparam name="U">parameters type</typeparam>
        /// <param name="query">sql query as string</param>
        /// <param name="parameters">parameters related to the sql query</param>
        /// <returns>Int: if greater than 0, sucessfully saved/updated</returns>
        public async Task<int> SaveDataAsync<U>(string query, U parameters)
        {
            using (IDbConnection conn = SqlConnection())
            {
                return await conn.ExecuteAsync(query, parameters,
                                commandType: CommandType.Text);
            }
        }
        #endregion
    }
}
