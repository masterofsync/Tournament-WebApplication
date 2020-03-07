using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<Tt>> LoadDataInTransactionUsingQueryAsync<Tt, U>(string storedProcedure, U parameters);
        Task<Tt> LoadSingleDataInTransactionUsingQueryAsync<Tt, U>(string sqlQuery, U parameters);
        Task<int> SaveDataInTransactionUsingQueryAsync<Tt>(string sqlQuery, Tt parameters);

        void CommitTransaction();
        void RollbackTransaction();
        void Dispose();
    }
}
