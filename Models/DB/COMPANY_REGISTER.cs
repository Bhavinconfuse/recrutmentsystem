//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Recrutment_system.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class COMPANY_REGISTER
    {
        public COMPANY_REGISTER()
        {
            this.COMPANY_HR_REGISTER = new HashSet<COMPANY_HR_REGISTER>();
            this.COMPANY_REGISTER_REFRENCES = new HashSet<COMPANY_REGISTER_REFRENCES>();
        }
    
        public int CompanyId { get; set; }
        public string ComapanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyContect { get; set; }
        public string CompanyAddress { get; set; }
    
        public virtual ICollection<COMPANY_HR_REGISTER> COMPANY_HR_REGISTER { get; set; }
        public virtual ICollection<COMPANY_REGISTER_REFRENCES> COMPANY_REGISTER_REFRENCES { get; set; }
    }
}
