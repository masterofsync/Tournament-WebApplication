using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ITeamsRepository:IRepository
    {
        //Task<IEnumerable<TeamContractModel>> GetAllTeamsOfSport(string sportName);

        Task<IActionResult> AddTeamAsync(TeamContractModel model);

        Task<TeamContractModel> GetTeamAsync(int id);
        Task<string> GetAssociatedUserIdForTeamAsync(int teamId);
        Task<IEnumerable<TeamContractModel>> GetAllTeamsForUser(string userId);
        Task<IActionResult> DeleteTeamAsync(int id);
        Task<IActionResult> UpdateTeamAsync(TeamContractModel model);
    }
}
