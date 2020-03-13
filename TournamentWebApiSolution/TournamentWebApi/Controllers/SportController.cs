using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Infrastructure.Dapper.Repositories;
using Contract.Models;
using System.Net;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    //[Authorize]
    //[ApiVersion("1.0")]
    //[ApiVersion("1.1")]
    public class SportController : ControllerBase
    {
        private readonly ISportsRepository sportRepo;
        public SportController(ISportsRepository sportRepo)
        {
            this.sportRepo = sportRepo;
        }

        /// <summary>
        /// POST: api/Sport. Add a sport.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddSportAsync(SportContractModel model)
        {
            try
            {
                return await sportRepo.AddSportAsync(model);
            }
            catch (Exception)
            {
                // Log??
                return BadRequest();
            }
        }

        /// <summary>
        /// GET: api/Sport . Gets all sports
        /// </summary>
        /// <returns>Sport Data</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<SportContractModel>>> GetAllSportAsync()
        {
            try
            {
                var model = await sportRepo.GetAllSportAsync();

                // Cannot return IEnumerable type so need to convert to list.
                return model.ToList(); 
            }
            catch (Exception)
            {
                // Log??
                return BadRequest();
            }
        }

        /// <summary>
        /// GET: api/Sport/1. Get a sport given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Sport Data</returns>
        [HttpGet("{id}")]
        //[MapToApiVersion("1.1")]
        public async Task<ActionResult<SportContractModel>> GetSportAsync(int id)
        {
            try
            {
                var model = await sportRepo.GetSportAsync(id);
                model.Name=null;
                return model;
            }
            catch (Exception)
            {
                // Log??
                return BadRequest("test");
            }
        }

        //[HttpGet("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[MapToApiVersion("1.0")]
        //public async Task<ActionResult<SportContractModel>> GetSportAsyncVersion1(int id)
        //{
        //    try
        //    {
        //        var model = await sportRepo.GetSportAsync(id);

        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        // Log??
        //        return BadRequest();
        //    }
        //}

        /// <summary>
        /// PUT: api/Sport. Update a given sport.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateSportAsync(SportContractModel model)
        {
            try
            {
                //var sportModel = new SportContractModel() {SportId=1, Description = "Sport To Play", Name = "Soccer" }; // Example

                // update this to proper return later
                return await sportRepo.UpdateSportAsync(model);
            }
            catch (Exception)
            {
                // Log??
                return BadRequest();
            }
        }

        /// <summary>
        /// DELETE: api/Sport/1. Delete a Sport given id.
        /// </summary>
        /// <param name="id">integer</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteSportAsync(int id)
        {
            try
            {
                return await sportRepo.DeleteSportAsync(id);
            }
            catch (Exception)
            {
                // Log??
                return BadRequest();
            }
        }
    }
}