using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
    }
}