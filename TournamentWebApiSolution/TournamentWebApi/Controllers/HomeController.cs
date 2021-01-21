using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TournamentWebApi.Models;
using DotNetTools.Adapter;
using TournamentWebApi.Infrastructure.Dapper.Repositories;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUserFromIdentityModel> _userManager;

        private readonly string BaseUri = "https://localhost:5001/api/";

        public HomeController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUserFromIdentityModel> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Test(string firstName)
        {
            var newModel = new UserLoginModel()
            {
                firstName = firstName
            };

            var content = new StringContent(JsonConvert.SerializeObject(newModel), Encoding.UTF8, "application/json");
            var url = "User/Test";

            var post= await HttpClientAdapter.Post(BaseUri +url, content);
            var data= await HttpClientAdapter.GetAsync<UserLoginModel>(BaseUri+"User");

            return Content($"Hello {data.firstName}");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";


            //SportRepository sportRepo = new SportRepository();

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            //string[] roles = { "Admin" };

            //var roleExist = await _roleManager.RoleExistsAsync(roles[0]);

            //if (roleExist == false)
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(roles[0]));
            //}

            //var user = await _userManager.FindByEmailAsync("bikzzzzz@gmail.com");

            //if (user != null)
            //{
            //    await _userManager.AddToRoleAsync(user, "Admin");
            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
