using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly UserManager<IdentityUser> userManager;

        public BaseController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        protected async Task<IdentityUser> GetIdentityUser()
        {
            IdentityUser user = await userManager.GetUserAsync(HttpContext.User);

            return user;
        }
    }
}