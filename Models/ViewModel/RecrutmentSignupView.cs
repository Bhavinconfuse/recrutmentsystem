using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Recrutment_system.Models.ViewModel
{
    public class RecrutmentSignupView
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name="Company Name")]
        public string ComapanyName { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name="Company Email")]
        public string CompanyEmail { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage="*")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name="Company Conetct")]
        public string CompanyContect { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }


        public string CompanyEmailRef { get; set; }

        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,20}", ErrorMessage = @"Error. Password must have one capital, one special character and one numerical character. It can not start with a special character or a digit.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string CompanyPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("CompanyPassword", ErrorMessage = "The password and confirmation password do not match.")] //Why not display this message???????
        public string ConfirmPassword { get; set; }
        public int ComapnyrefId { get; set; }

    }

    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
    }
}