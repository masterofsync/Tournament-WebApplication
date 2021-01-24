using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Infrastructure.Dapper.Repositories;
using TournamentWebApi.Models;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    //[Authorize]
    public class TeamController : BaseController
    {
        private readonly ITeamsRepository teamRepo;

        public TeamController(ITeamsRepository teamRepo, UserManager<ApplicationUserFromIdentityModel> userManager) : base(userManager)
        {
            this.teamRepo = teamRepo;
        }

        /// <summary>
        /// POST: api/Teams. Add a team
        /// </summary>
        /// <param name="model">TeamContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddTeamAsync(TeamContractModel model)
        {
            try
            {
                //TODO::check for current user and not from the model

                // Create teamstatsId for the team
                return await teamRepo.AddTeamAsync(model);
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }

        /// <summary>
        /// DELETE: api/Teams. Delete team
        /// </summary>
        /// <param name="model">TeamContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTeamAsync(int teamId)
        {
            try
            {
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), teamId) != false)
                {
                    // Create teamstatsId for the team
                    return await teamRepo.DeleteTeamAsync(teamId);
                }

                //not authorized?? or not found??
                return NotFound();
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }

        /// <summary>
        /// PUT: api/Teams. Update team
        /// </summary>
        /// <param name="model">TeamContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTeamAsync(TeamContractModel teamModel)
        {
            try
            {
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), teamModel.TeamId) != false)
                {
                    // Create teamstatsId for the team
                    return await teamRepo.UpdateTeamAsync(teamModel);
                }

                //not authorized?? or not found??
                return NotFound();
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }

        /// <summary>
        /// GET: api/Teams. Get Team
        /// </summary>
        /// <param name="model">TeamContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTeamAsync(int teamId)
        {
            try
            {
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), teamId) != false)
                {
                    // return team with ok code.
                    return Ok(await teamRepo.GetTeamAsync(teamId));
                }

                //not authorized?? or not found??
                return NotFound();
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }

        /// <summary>
        /// GET: api/Teams. Get all teams for user
        /// </summary>
        /// <param name="model">TeamContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllTeamForUserAsync()
        {
            try
            {
                var currentUser = await GetCurrentUser();
                var repo = await teamRepo.GetAllTeamsForUser(currentUser.Id);

                if (repo != null)
                {
                    return Ok(repo);
                }
                //not authorized?? or not found??
                return NotFound();
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }

        /// <summary>
        /// Check if user is admin or authorized to update/delete a team.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private async Task<bool> CheckIfAuthorized(ApplicationUserFromIdentityModel user, int teamId)
        {
            try
            {
                var team = await teamRepo.GetTeamAsync(teamId);
                if (await _userManager.IsInRoleAsync(user, "Admin") || String.Equals(team.UserId, user.Id))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}