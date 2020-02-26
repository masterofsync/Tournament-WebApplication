using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface IRepository<ObjectType> where ObjectType : class
    {
        ObjectType Get(int id);
        IEnumerable<ObjectType> GetAll();
        IEnumerable<ObjectType> Find(Expression<Func<ObjectType, bool>> predicate);

        void Add(ObjectType o);
        void AddRange(IEnumerable<ObjectType> o);

        void Remove(ObjectType o);
        void RemoveRange(IEnumerable<ObjectType> o);
    }
}
