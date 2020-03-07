using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ISportRepository : IRepository<SportContractModel>
    {
        Task<IEnumerable<SportContractModel>> GetAllSport();
        Task<SportContractModel> GetSport(int id);

        Task<IActionResult> AddSportAsync(SportContractModel model);
        Task<IActionResult> DeleteSportAsync(int id);
        Task<IActionResult> UpdateSportAsync(SportContractModel model);
    }
}
