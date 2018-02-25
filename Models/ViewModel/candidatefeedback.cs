using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Recrutment_system.Models.ViewModel
{
    public class candidatefeedback
    {
        [Key]
        public string candidatecode { get; set; }

        public SelectList candicode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Total Round Percentage")]
        public int rounding_percent { get; set; }


        [Required(ErrorMessage="Please Add For Feedback 'first' round candidate interview")]
        [Display(Name="Round One Feedback")]
        public string round1 { get; set; }

        [Display(Name = "Round one %")]
        [Required]
        [Range(5, 20)]      
        public int round1P { get; set; }




        [Required(ErrorMessage = "Please Add For Feedback 'second' round candidate interview")]
        [Display(Name = "Round Two Feedback")]
        public string round2 { get; set; }

        [Display(Name = "Round two %")]
        [Required]
        [Range(5, 40)]
        public int round2P { get; set; }




        [Required(ErrorMessage = "Please Add For Feedback 'third' round candidate interview")]
        [Display(Name = "Round Three Feedback")]

        public string round3 { get; set; }
        [Display(Name = "Round threee %")]
        [Required]
        [Range(5, 40)]
        public int round3P { get; set; }


        //after get the record for the information 

        public string candidateemail { get; set; }
        public string yerofexperience { get; set; }
        public string Depetcode { get; set; }
        public string Departmentname { get; set; }
        public string nameofvacancy { get; set; }
        public decimal numberofpostion { get; set; }
        public string reqtechnicalskill { get; set; }
        public string specialskill { get; set; }
        public string minyerofexp { get; set; }

        public string minqualifi { get; set; }
        public string deadlinedate { get; set; }


    }

    public class Listofoffer
    {
        public IEnumerable<candidatefeedback> listoffer{get;set;}
    }
}