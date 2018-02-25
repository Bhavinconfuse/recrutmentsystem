using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Recrutment_system.Models.ViewModel
{
    public class UserSignup
    {
        [Key]
        public int HrId { get; set; }

        public int CompanyId { get; set; }
        [Required(ErrorMessage="*")]
        [Display(Name="Add Name")]
        public string HrName { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name="Add Address")]
        public string HrContact { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name="Add Email")]
        [DataType(DataType.EmailAddress)]
        public string HrEmail { get; set; }


    }
}