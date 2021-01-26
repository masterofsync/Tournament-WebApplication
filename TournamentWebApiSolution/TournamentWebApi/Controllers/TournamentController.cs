using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contract.Models;
using TournamentWebApi.Infrastructure.Dapper.Repositories;
using TournamentWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    //[Authorize]
    public class TournamentController : BaseController
    {
        private readonly ITournamentsRepository tournamentRepo;
        public TournamentController(ITournamentsRepository repo, UserManager<ApplicationUserFromIdentityModel> userManager) : base(userManager)
        {
            tournamentRepo = repo;
        }

        /// <summary>
        /// POST: api/Tournament. Add a tournament
        /// </summary>
        /// <param name="model">TournamentContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddTournamentAsync(TournamentContractModel model)
        {
            try
            {
                //Check for current user and not from the model
                //var currentUser = await GetCurrentUser();

                //set current userId
                //model.UserId = currentUser.Id;
                //Testing
                model.UserId = model.UserId;

                //TODO: Get id of default point sytem.
                if(model.TournamentPointSystemIdContractModel.DefaultPointSystem==true)
                {
                    model.TournamentPointSystemIdContractModel.TournamentPointSystemId = 1;
                }
                else
                {
                    model.TournamentPointSystemIdContractModel.TournamentPointSystemId = await tournamentRepo.CreateSubmittedPointSystem(model.TournamentPointSystemIdContractModel);
                }

                // Create teamstatsId for the team
                return await tournamentRepo.AddTournamentAsync(model);
            }
            catch (Exception)
            {
                //log??
                return BadRequest();
            }
        }



        /// <summary>
        /// GET: api/Tournament. Get a specific tournament
        /// </summary>
        /// <param name="tournamentId">tournament id</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTournamentAsync(int tournamentId)
        {
            try
            {
                var UserIdOfTournament = await tournamentRepo.GetAssociatedUserIdForTournamentAsync(tournamentId);
                // Check if the corresponding team is of that user.
                if (await CheckIfAuthorized(await GetCurrentUser(), UserIdOfTournament) != false)
                {
                    // return team with ok code.
                    return Ok(await tournamentRepo.GetTournamentAsync(tournamentId));
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
        /// GET: api/Tournament. Get all tournament for user.
        /// </summary>
        /// <param name="model">TeamContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllTournamentsForUserAsync()
        {
            try
            {
                var currentUser = await GetCurrentUser();
                var repo = await tournamentRepo.GetAllTournamentsForUser(currentUser.Id);

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

        //TODO:
        // Update
        // Delete


        #region Tournament type

        /// <summary>
        /// POST: api/tournament/type. Add tournament type.
        /// </summary>
        /// <param name="model">TournamentType model</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPost("type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddTypeAsync(TournamentTypeContractModel model)
        {
            try
            {
                return await tournamentRepo.AddTypeAsync(model);
            }
            catch (Exception)
            {
                return BadRequest();
                //throw;
            }
        }

        /// <summary>
        /// GET: api/tournament/type. Get tournament type given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TournamentType Model</returns>
        [HttpGet("type/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TournamentTypeContractModel>> GetTypeAsync(int id)
        {
            try
            {
                var model = await tournamentRepo.GetTypeAsync(id);
                return model;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// GET: api/tournament/type. Get all tournament type
        /// </summary>
        /// <returns>List of TournamentType model</returns>
        [HttpGet("Type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<TournamentTypeContractModel>>> GetAllTypeAsync()
        {
            try
            {
                var model = await tournamentRepo.GetAllTypeAsync();
                return model.ToList();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// DELETE: api/tournament/type. Delete tournament type given id
        /// </summary>
        /// <param name="id">integer</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpDelete("Type/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTypeAsync(int id)
        {
            try
            {
                return await tournamentRepo.DeleteTypeAsync(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// PUT: api/tournament/type. Update tournament type given id
        /// </summary>
        /// <param name="model">Tournament type model</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPut("Type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTypeAsync(TournamentTypeContractModel model)
        {
            try
            {
                return await tournamentRepo.UpdateTypeAsync(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}