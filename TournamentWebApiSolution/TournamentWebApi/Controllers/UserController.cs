using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentWebApi.Data;
using TournamentWebApi.Models;
using Contract.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
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

        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return Ok(new UserLoginModel() { firstName="test"});
        //}


        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        // POST api/User
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterUserContractModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var user = new ApplicationUserFromIdentityModel
                {
                    UserName = userModel.UserName,
                    Email = userModel.Email,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    PhoneNumber = userModel.PhoneNumber,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                // return specifics?
                // return Created();
                return Ok();
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        //TO DO:
        // Login
        // Log out
        //


        [HttpGet]
        public async Task<ApplicationUserModel> GetByUserName()
        
        {
            string userName = "Bikesh707@gmail.com";
            var user = await _userManager.FindByEmailAsync(userName);
            //UserData data = new UserData();
            //var name = _context.Users.Find(userName);
            //var user = await _userManager.GetUserName(userName);
            return new ApplicationUserModel() { Email=user.Email,Id=user.Id};
            //return Ok(/*user.NormalizedEmail*/);
        }

        [HttpPost]
        [Route("Test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Test(RegisterUserContractModel test)
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


        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}