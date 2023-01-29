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
    
    public partial class tbl_Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Customer()
        {
            this.tbl_Cart = new HashSet<tbl_Cart>();
            this.tbl_CusReview = new HashSet<tbl_CusReview>();
            this.tbl_PhoneNumbers = new HashSet<tbl_PhoneNumbers>();
            this.tbl_FavRestaurants = new HashSet<tbl_FavRestaurants>();
            this.tbl_Addresses = new HashSet<tbl_Addresses>();
        }
    
        public int CusId { get; set; }
        public string CusName { get; set; }
        public string CusEmail { get; set; }
        public string CusPassword { get; set; }
        public string CusImage { get; set; }
        public string CusPincode { get; set; }
        public string CusStatus { get; set; }
        public Nullable<int> CusRole { get; set; }
        public string IsValid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Cart> tbl_Cart { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_CusReview> tbl_CusReview { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PhoneNumbers> tbl_PhoneNumbers { get; set; }
        public virtual tbl_Role tbl_Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_FavRestaurants> tbl_FavRestaurants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Addresses> tbl_Addresses { get; set; }
    }
}
