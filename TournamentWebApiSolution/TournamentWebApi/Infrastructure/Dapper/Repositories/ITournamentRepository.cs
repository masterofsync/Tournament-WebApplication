using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ITournamentRepository : IRepository
    {
        Task<IActionResult> AddTypeAsync(TournamentTypeContractModel model);
        Task<IEnumerable<TournamentTypeContractModel>> GetAllTypeAsync();
        Task<TournamentTypeContractModel> GetTypeAsync(int id);
        Task<IActionResult> DeleteTypeAsync(int id);
        Task<IActionResult> UpdateTypeAsync(TournamentTypeContractModel model);
    }
}
