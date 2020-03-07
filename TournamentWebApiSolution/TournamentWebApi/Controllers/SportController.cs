using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Infrastructure.Dapper.Repositories;
using Contract.Models;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SportController : ControllerBase
    {
        private readonly ISportRepository sportRepo;
        public SportController(ISportRepository sportRepo)
        {
            this.sportRepo = sportRepo;
            //this.UpdateSportAsync( new SportContractModel() { SportId = 1, Description = "Sport To Play", Name = "Soccer" });
        }

        [HttpPost]
        public async Task AddSportAsync(SportContractModel model)
        {
            try
            {
                var sportModel = new SportContractModel() { Description = "Best Sport", Name = "Soccer" }; // Example

                await sportRepo.AddSportAsync(sportModel);
            }
            catch (Exception)
            {
                throw;// update this to proper return later
            }
        }

        // GET: api/Sport . Gets all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SportContractModel>>> GetAllSportAsync()
        {
            try
            {
                var model = await sportRepo.GetAllSport();

                // Cannot return IEnumerable type so need to convert to list.
                return model.ToList(); 
            }
            catch (Exception)
            {
                return Ok();// update this to proper return later
                throw;
            }
        }

        // GET: api/Sport/1
        [HttpGet("{id}")]
        public async Task<ActionResult<SportContractModel>> GetSportAsync(int id)
        {
            try
            {
                var model = await sportRepo.GetSport(id);

                return model;
            }
            catch (Exception)
            {
                // update this to proper return later
                throw;
            }
        }

        [HttpPut]
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
                throw;// update this to proper return later
            }
        }
    }
}