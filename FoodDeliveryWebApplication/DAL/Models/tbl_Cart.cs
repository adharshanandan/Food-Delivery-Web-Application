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
    
    public partial class tbl_Cart
    {
        public int CartId { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Cart_fk_DishId { get; set; }
        public Nullable<int> Cart_fk_CusId { get; set; }
        public Nullable<int> Cart_fk_RestId { get; set; }
    
        public virtual tbl_Dishes tbl_Dishes { get; set; }
        public virtual tbl_Customer tbl_Customer { get; set; }
        public virtual tbl_Restaurant tbl_Restaurant { get; set; }
    }
}
