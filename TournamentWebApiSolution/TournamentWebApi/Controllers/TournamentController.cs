using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contract.Models;
using TournamentWebApi.Infrastructure.Dapper.Repositories;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentRepository tournamentRepo;
        public TournamentController(ITournamentRepository repo)
        {
            tournamentRepo = repo;
        }

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