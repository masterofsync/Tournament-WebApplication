using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Infrastructure.Dapper.Repositories;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class TeamController : BaseController
    {
        private readonly ITeamsRepository teamRepo;

        public TeamController(ITeamsRepository teamRepo, UserManager<IdentityUser> userManager):base(userManager)
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
                // Create teamstatsId for the team
                return await teamRepo.AddTeamAsync(model);
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }
        // Delete team
        // Update team
        // Get Team
        // Get all Team for user
        // Get all Team for user and Sport
    }
}