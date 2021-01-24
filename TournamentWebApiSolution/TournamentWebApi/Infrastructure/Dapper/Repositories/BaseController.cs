using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Models;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly UserManager<ApplicationUserFromIdentityModel> _userManager;

        public BaseController(UserManager<ApplicationUserFromIdentityModel> userManager)
        {
            this._userManager = userManager;
        }

        protected async Task<ApplicationUserFromIdentityModel> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return user;
        }
    }
}