using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<T>> LoadDataInTransactionUsingQueryAsync<T, U>(string storedProcedure, U parameters);
        Task<T> LoadSingleDataInTransactionUsingQueryAsync<T, U>(string sqlQuery, U parameters);
        Task<int> SaveDataInTransactionUsingQueryAsync<T>(string sqlQuery, T parameters);

        void CommitTransaction();
        void RollbackTransaction();
        void Dispose();
    }
}
