﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RecrutmentsysEntities : DbContext
    {
        public RecrutmentsysEntities()
            : base("name=RecrutmentsysEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<COMPANY_REGISTER> COMPANY_REGISTER { get; set; }
        public DbSet<COMPANY_HR_REGISTER> COMPANY_HR_REGISTER { get; set; }
        public DbSet<COMPANY_JOB_OPENING> COMPANY_JOB_OPENING { get; set; }
        public DbSet<COMPANY_JOB_OPENING_REFRENCES> COMPANY_JOB_OPENING_REFRENCES { get; set; }
        public DbSet<CANDIDATE_POSITION> CANDIDATE_POSITION { get; set; }
        public DbSet<Round_Detail_feedback> Round_Detail_feedback { get; set; }
        public DbSet<CANDIDATE_FEEDBACK_POSITION> CANDIDATE_FEEDBACK_POSITION { get; set; }
        public DbSet<COMPANY_REGISTER_REFRENCES> COMPANY_REGISTER_REFRENCES { get; set; }
    }
}
