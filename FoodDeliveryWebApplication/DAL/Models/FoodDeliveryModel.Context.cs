﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_FoodOrderingApplicationEntities : DbContext
    {
        public db_FoodOrderingApplicationEntities()
            : base("name=db_FoodOrderingApplicationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Admin> tbl_Admin { get; set; }
        public virtual DbSet<tbl_Cart> tbl_Cart { get; set; }
        public virtual DbSet<tbl_Category> tbl_Category { get; set; }
        public virtual DbSet<tbl_CusReview> tbl_CusReview { get; set; }
        public virtual DbSet<tbl_Customer> tbl_Customer { get; set; }
        public virtual DbSet<tbl_CustomerAddresses> tbl_CustomerAddresses { get; set; }
        public virtual DbSet<tbl_DeliveryStaffs> tbl_DeliveryStaffs { get; set; }
        public virtual DbSet<tbl_Dishes> tbl_Dishes { get; set; }
        public virtual DbSet<tbl_Offers> tbl_Offers { get; set; }
        public virtual DbSet<tbl_PhoneNumbers> tbl_PhoneNumbers { get; set; }
        public virtual DbSet<tbl_Restaurant> tbl_Restaurant { get; set; }
        public virtual DbSet<tbl_ContactUs> tbl_ContactUs { get; set; }
        public virtual DbSet<tbl_Login> tbl_Login { get; set; }
        public virtual DbSet<tbl_Role> tbl_Role { get; set; }
        public virtual DbSet<tbl_FavRestaurants> tbl_FavRestaurants { get; set; }
    }
}
