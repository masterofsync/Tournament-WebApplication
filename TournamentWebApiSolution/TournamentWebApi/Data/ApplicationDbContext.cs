using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TournamentWebApi.Models;

namespace TournamentWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserFromIdentityModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
