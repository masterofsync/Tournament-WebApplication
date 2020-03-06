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

        public List<Tt> LoadData<Tt, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (IDbConnection connection = SqlConnection())
            {
                List<Tt> rows = connection.Query<Tt>(storedProcedure, parameters,
                                commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<Tt>(string storedProcedure, Tt parameters, string connectionStringName)
        {
            using (IDbConnection connection = SqlConnection())
            {
                connection.Execute(storedProcedure, parameters,
                                    commandType: CommandType.StoredProcedure);
            }
        }

        public void StartTransaction()
        {
            _connection = SqlConnection();
            _connection.Open();

            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        public IEnumerable<Tt> LoadDataInTransactionUsingStoredProcedure<Tt, U>(string storedProcedure, U parameters)
        {
            List<Tt> rows = _connection.Query<Tt>(storedProcedure, parameters,
                            commandType: CommandType.StoredProcedure, transaction: _transaction).ToList(); 
            return rows;
        }

        public IEnumerable<Tt> LoadDataInTransactionUsingQuery<Tt, U>(string sqlQuery, U parameters)
        {
            List<Tt> rows = _connection.Query<Tt>(sqlQuery, parameters,
                            commandType: CommandType.Text, transaction: _transaction).ToList();
            return rows;
        }

        public void SaveDataInTransactionUsingStoredProcedure<Tt>(string storedProcedure, Tt parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void SaveDataInTransactionUsingQuery<Tt>(string sqlQuery, Tt parameters)
        {
            _connection.Execute(sqlQuery, parameters,
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
