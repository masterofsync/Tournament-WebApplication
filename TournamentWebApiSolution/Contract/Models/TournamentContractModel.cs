using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contract.Models
{
    public class TournamentContractModel
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public int SportId { get; set; }
        //public string Sport { get; set; }

        public SportContractModel Sport { get; set; }
        //public int TournamentTypeId { get; set; }
        //public string TournamentType { get; set; }

        public TournamentTypeContractModel TournamentType { get; set; }
        public TournamentPointSystemIdContractModel TournamentPointSystemIdContractModel { get; set; }
        public string UserId { get; set; }
    }

    public class TournamentTypeContractModel
    {
        public int TournamentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TournamentPointSystemIdContractModel
    {
        public int TournamentPointSystemId { get; set; }
        public string Name { get; set; }
        public int Winpoint { get; set; }
        public int DrawPoint { get; set; }
        public int LossPoint { get; set; }

        [Required]
        // This is required to check if default or submitted values to choose.
        public bool DefaultPointSystem { get; set; }
    }


}
