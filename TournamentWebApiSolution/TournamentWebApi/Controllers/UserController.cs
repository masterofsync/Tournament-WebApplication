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
using System.Web;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Text;
using Contract.Models.UserModels;

namespace TournamentWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUserFromIdentityModel> _userManager;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUserFromIdentityModel> userManager, IConfiguration config)
        {
            _context = context;
            this._userManager = userManager;
            this._config = config;
        }

        #region User Registration and Confirmation

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


        /// <summary>
        /// Get confirmation code given registered email and password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetConfirmationCode(UserContractUserModel model)
        {
            try
            {
                if (model != null & model.Email != null)
                {
                    ApplicationUserFromIdentityModel user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var checkUserPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (checkUserPassword)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            return Ok(code);
                        }
                    }
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                ApplicationUserFromIdentityModel user = await _userManager.FindByIdAsync(userId);
                if (user.EmailConfirmed)
                {
                    return Ok("Email already Confirmed!");
                }

                if (user != null)
                {
                    var result = await _userManager.ConfirmEmailAsync(user, code);

                    if (result.Succeeded)
                        return Ok("Confirmed");
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        /// <summary>
        /// Change password by currently logged on user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword(ChangePasswordContractUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Get forgot Code to reset password.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetForgotPasswordCode")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForgotPasswordCode(string email)
        {
            var model = new ForgotPasswordContractUserModel();
            if (ModelState.IsValid)
            {
                ApplicationUserFromIdentityModel user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    model.Code = HttpUtility.UrlEncode(code);
                    model.Email = email;
                    return Ok(code);
                }
                return NotFound();
            }
            return BadRequest();
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="model">EmailContractModel</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SendEmail(EmailContractModel model)
        {
            try
            {
                if (model != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(model.Email);
                    mail.From = new MailAddress(_config["Email"]);
                    mail.Subject = model.Subject;
                    string Body = model.Content;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                        (_config["Email"], _config["Password"]); // TODO: setup new user pass 
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    return Ok();
                }
                return BadRequest("Model is null!");
            }
            catch (Exception)
            {
                return BadRequest("Model is null!");
            }
        }

        // TODO: External Login (gmail??)

        #region Admin
        /// <summary>
        /// Reset Password for user. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/ResetPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordContractUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserId != null)
                {
                    ApplicationUserFromIdentityModel user = await _userManager.FindByIdAsync(model.UserId);
                    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }


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

        #endregion

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