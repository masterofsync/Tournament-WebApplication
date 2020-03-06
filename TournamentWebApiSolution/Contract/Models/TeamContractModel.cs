using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.Models
{
    public class TeamContractModel
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public int SportId { get; set; }
        public string  Sport { get; set; }

        public int TeamStatsId { get; set; }
    }
}
