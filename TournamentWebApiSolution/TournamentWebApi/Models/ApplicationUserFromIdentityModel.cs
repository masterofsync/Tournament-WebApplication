using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentWebApi.Models
{
    public class ApplicationUserFromIdentityModel: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private string PhoneNumber { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
