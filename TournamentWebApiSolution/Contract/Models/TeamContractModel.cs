using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contract.Models
{
    public class TeamContractModel
    {
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int TeamStatsId { get; set; }
        public string UserId { get; set; }
    }

    public class TeamStatsContractModel
    {
        public int TeamStatsId { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public int matchesPlayed { get; set; }
        public int goalsFor { get; set; }
        public int goalsAgainst { get; set; }
    }
}
