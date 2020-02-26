using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Data;
using TournamentWebApi.Models;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(new UserLoginModel() { firstName="test"});
        }

        //[HttpGet]
        //public UserModel GetById()
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    UserData data = new UserData();

        //    return data.GetUserById(userId).First(); 
        //}

        [HttpPost]
        [Route("Test")]
        public IActionResult Test(UserLoginModel test)
        {
            if(test!=null) return Ok();

            return BadRequest();
        }

        //[HttpPost]
        //public ActionResult TestMethod(string firstName)
        //{
        //    // Do something with username and password
        //    var user = firstName;
        //    //var passWord = password;
        //    return null;
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> userList = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var uRoles = from userRoles in _context.UserRoles
                         join r in _context.Roles on userRoles.RoleId equals r.Id
                         select new { userRoles.UserId, userRoles.RoleId, r.Name };


            foreach (var user in users)
            {
                ApplicationUserModel userModel = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                userModel.Roles = uRoles.Where(x => x.UserId == userModel.Id)
                                        .ToDictionary(key => key.RoleId, val => val.Name);

                userList.Add(userModel);
            }
            return userList;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public async Task RemoveARole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}