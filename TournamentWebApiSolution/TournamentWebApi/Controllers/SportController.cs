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
        }

        [HttpPost]
        public void AddSport(SportContractModel model)
        {
            var sportModel = new SportContractModel() { Description="Best Sport",Name="Soccer"}; // Example

            sportRepo.AddSport(sportModel);
        }

        // GET: api/Sport/1
        [HttpGet("{id}")]
        public async Task<ActionResult<SportContractModel>> GetSport(int id)
        {
            var model = await sportRepo.GetAllSport
        }
    }
}