using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Recrutment_system.Models.ViewModel
{
    public class UserLoginView
    {
        [Key]
        public int CompanyId { get; set; }
        [Required(ErrorMessage="*")]
        [Display(Name="Login Id")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class ResetPWD
    {
        [Key]
        [Required(ErrorMessage="*")]
        [Display(Name="Email Id")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
    }
    public class forgetpassword
    {
        [Key]
        public int refid { get; set; }
        [Required(ErrorMessage="*")]
        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,20}", ErrorMessage = @"Error. Password must have one capital, one special character and one numerical character. It can not start with a special character or a digit.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string newpassword { get;set;}

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("newpassword", ErrorMessage = "The password and confirmation password do not match.")] //Why not display this message???????
        public string confirmpassword { get; set; }

        public string mail { get; set; }

        
    }
}