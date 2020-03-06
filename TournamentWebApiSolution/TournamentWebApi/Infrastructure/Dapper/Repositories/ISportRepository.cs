using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ISportRepository : IRepository<SportContractModel>
    {
        IEnumerable<SportContractModel> GetAllSport();
        SportContractModel GetSport(int id);

        bool AddSport(SportContractModel model);
        bool DeleteSport(SportContractModel model);
        bool UpdateSport(SportContractModel model);
    }
}
