using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contract.Models.UserModels
{
    public class ForgotPasswordContractUserModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Code { get; set; }
    }
}
