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
        IEnumerable<Tt> LoadDataInTransactionUsingStoredProcedure<Tt, U>(string storedProcedure, U parameters);
        IEnumerable<Tt> LoadDataInTransactionUsingQuery<Tt, U>(string storedProcedure, U parameters);
        void SaveDataInTransactionUsingStoredProcedure<Tt>(string storedProcedure, Tt parameters);
        void SaveDataInTransactionUsingQuery<Tt>(string sqlQuery, Tt parameters);
        void CommitTransaction();
        void RollbackTransaction();
        void Dispose();
    }
}
