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


        /// <summary>
        /// Check if user is admin or authorized to update/delete the item.
        /// </summary>
        /// <param name="currentUser">Current user of the httpcontext</param>currentUser
        /// <param name="correspondingUserId">User Id of the corresponding item</param>
        /// <returns></returns>
        protected async Task<bool> CheckIfAuthorized(ApplicationUserFromIdentityModel currentUser, string correspondingUserId)
        {
            try
            {
                if (await _userManager.IsInRoleAsync(currentUser, "Admin") || String.Equals(correspondingUserId, currentUser.Id))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}