using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ISportsRepository : IRepository
    {
        Task<IActionResult> AddSportAsync(SportContractModel model);
        Task<IEnumerable<SportContractModel>> GetAllSportAsync();
        Task<SportContractModel> GetSportAsync(int id);
        Task<IActionResult> DeleteSportAsync(int id);
        Task<IActionResult> UpdateSportAsync(SportContractModel model);
    }
}
