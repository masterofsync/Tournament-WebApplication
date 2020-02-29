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

        private readonly string _tableName;
        protected Repository(IConfiguration config)
        {
            _tableName = null;
            Configuration = config;
        }

        protected Repository(string tableName, IConfiguration config)
        {
            _tableName = tableName;
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

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

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

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction()
        {
            _connection = SqlConnection();

            _transaction = _connection.BeginTransaction();
        }

        public List<Tt> LoadDataInTransaction<Tt, U>(string storedProcedure, U parameters)
        {
            List<Tt> rows = _connection.Query<Tt>(storedProcedure, parameters,
                            commandType: CommandType.StoredProcedure, transaction: _transaction).ToList(); 
            return rows;
        }

        public void SaveDataInTransaction<Tt>(string storedProcedure, Tt parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }
        public void Dispose()
        {
            CommitTransaction();
        }

        #region Interface Implementation
        public async Task<T> GetAsync(Guid id)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id });

                if (result == null)
                {
                    throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
                }

                return result;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM{_tableName}");
            }
        }

        public async Task DeleteRowAsync(Guid id)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
            }
        }

        public async Task AddAsync(T t)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
