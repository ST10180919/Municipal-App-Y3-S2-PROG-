﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Municipal_App
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MunicipalDatabaseEntities : DbContext
    {
        public MunicipalDatabaseEntities()
            : base("name=MunicipalDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ATTACHMENT> ATTACHMENTs { get; set; }
        public virtual DbSet<ISSUE_REPORT> ISSUE_REPORT { get; set; }
    }
}
