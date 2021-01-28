using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ITournamentsRepository : IRepository
    {
        Task<IActionResult> AddTournamentAsync(TournamentContractModel model);
        Task<TournamentContractModel> GetTournamentAsync(int tournamentId);
        Task<IEnumerable<TournamentContractModel>> GetAllTournamentsForUser(string userId);
        Task<int> CreateSubmittedPointSystem(TournamentPointSystemContractModel model);
        Task<string> GetAssociatedUserIdForTournamentAsync(int tournamentId);
        Task<IActionResult> UpdateTournamentAsync(TournamentContractModel model);
        Task<IActionResult> DeleteTournamentAsync(int tournamentId);
        Task<IActionResult> AddTypeAsync(TournamentTypeContractModel model);
        Task<IEnumerable<TournamentTypeContractModel>> GetAllTypeAsync();
        Task<TournamentTypeContractModel> GetTypeAsync(int id);
        Task<IActionResult> DeleteTypeAsync(int id);
        Task<IActionResult> UpdateTypeAsync(TournamentTypeContractModel model);
    }
}
