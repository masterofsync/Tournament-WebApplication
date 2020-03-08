using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.Models
{
    public class TournamentContractModel
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        public string Sport { get; set;}
        public int TournamentTypeId { get; set; }
        public string TournamentType { get; set; }

        //public int TournamentPointSystemId { get; set; }

    }

    public class TournamentTypeContractModel
    {
        public int TournamentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
