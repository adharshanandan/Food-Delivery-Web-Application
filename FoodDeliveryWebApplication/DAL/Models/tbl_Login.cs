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
    
    public partial class tbl_Login
    {
        public int LoginId { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public Nullable<int> UserRole { get; set; }
    
        public virtual tbl_Role tbl_Role { get; set; }
    }
}
