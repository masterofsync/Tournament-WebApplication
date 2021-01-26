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
                //Check for current user and not from the model
                var currentUser = await GetCurrentUser();

                //set current userId
                model.UserId = currentUser.Id;

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
                var UserIdOfTeam = await teamRepo.GetAssociatedUserIdForTeamAsync(teamId);
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), UserIdOfTeam) != false)
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
                var UserIdOfTeam = await teamRepo.GetAssociatedUserIdForTeamAsync(teamModel.TeamId);
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), UserIdOfTeam) != false)
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
                var UserIdOfTeam = await teamRepo.GetAssociatedUserIdForTeamAsync(teamId);
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), UserIdOfTeam) != false)
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
    }
}