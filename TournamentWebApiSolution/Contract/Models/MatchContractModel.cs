using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.Models
{
    public class MatchContractModel
    {
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public int HomeTeamId { get; set; }
        public String HomeTeamName { get; set; }
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
    }
}
