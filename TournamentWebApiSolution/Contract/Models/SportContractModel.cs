using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contract.Models
{
    public class SportContractModel
    {
        public int SportId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
