using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
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

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(Configuration["AzureTournamentDBCOnnection"]);
        }

        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }

        public void StartTransaction()
        {
            _connection = SqlConnection();
            _connection.Open();

            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        public async Task<IEnumerable<Tt>> LoadDataInTransactionUsingQueryAsync<Tt, U>(string sqlQuery, U parameters)
        {
            var rows = await _connection.QueryAsync<Tt>(sqlQuery, parameters,
                            commandType: CommandType.Text, transaction: _transaction);
            return rows;
        }

        public async Task<Tt> LoadSingleDataInTransactionUsingQueryAsync<Tt, U>(string sqlQuery, U parameters)
        {
            var row = await _connection.QuerySingleOrDefaultAsync<Tt>(sqlQuery, parameters,
                            commandType: CommandType.Text, transaction: _transaction);
            return row;
        }

        public async Task<int> SaveDataInTransactionUsingQueryAsync<Tt>(string sqlQuery, Tt parameters)
        {
            return await _connection.ExecuteAsync(sqlQuery, parameters,
                                commandType: CommandType.Text, transaction: _transaction);
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
    }
}
