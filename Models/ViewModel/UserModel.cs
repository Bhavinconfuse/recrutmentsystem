using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Recrutment_system.Models.ViewModel
{

    public class UserModel
    {
    }
    public class UsersJob
    {
        [Key]
        public int DepatmentID { get; set; }
        public int CompanyId { get; set; }

        public string DepartmentCode { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }
        
        public string HrEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Name Of Vacancy")]
        public string Name_of_Vacancy { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Number of position")]
        public int Number_of_Position { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Requierd Technical Skill")]
        public string  Required_Technical_Skill { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Special Skill")]
        public string Special_Technical_Skill { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Min Year of Expirence")]
        public double Min_Yearof_Experience { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Min Qualification")]
        public string Min_Qualification { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Deadline Date")]
        [DataType(DataType.Date)]
        public DateTime Deadline_Date { get; set; }


        public DateTime Requested_Date { get; set; }

        public bool? Approval { get; set; }
        public bool Approval_admin { get; set; }
      
    }

    public class requested_by
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class hrRequested_by
    {
        public string selectby { get; set; }
        public IEnumerable<requested_by> requestd { get; set; }
    }
    public class listJobView
    {
        public IEnumerable<UsersJob> userjobs{get;set;}
        public hrRequested_by hr_requested_by { get; set; }

        
    }

    

    
}