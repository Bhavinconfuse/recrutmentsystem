using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Recrutment_system.Models.ViewModel
{
    public class candidatepostion
    {
        [Key]
        [Display(Name="Department Code")]
        public string departmentcode { get; set; }
            
        public SelectList Deptcode { get; set; }

        [Required(ErrorMessage="email must enter")]
        [Display(Name="Interviewer Email id")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name="Year of Experience")]
        public float year_of_experience { get; set; }

        public string Departmentname { get; set; }
        public int candi_yer_of_exper { get; set; }
        public string candidatecode { get; set; }

        public bool Isactive { get; set; }
        public int canid { get; set; }

    }
    public class ListofIsactive
    {
        public IEnumerable<candidatepostion> candidateposi { get; set; }

    }
    
}