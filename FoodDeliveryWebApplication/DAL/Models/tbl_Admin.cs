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
    
    public partial class tbl_Admin
    {
        public int AdmId { get; set; }
        public string AdmEmail { get; set; }
        public string AdmPassword { get; set; }
        public Nullable<int> AdminRole { get; set; }
    
        public virtual tbl_Role tbl_Role { get; set; }
    }
}
