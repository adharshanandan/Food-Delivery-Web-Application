//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class tbl_BankAccounts
    {
        public int AccId { get; set; }
        public Nullable<int> fk_BankName { get; set; }
        public string Branch { get; set; }
        public string AccNumber { get; set; }
        public string IfscCode { get; set; }
        public Nullable<decimal> AccBalance { get; set; }
        public string EmailId { get; set; }
    
        public virtual tbl_BankNames tbl_BankNames { get; set; }
    }
}