using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentWebApi.Models
{
    public class UserLoginInfoModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string grantType { get; set; }
    }
}
