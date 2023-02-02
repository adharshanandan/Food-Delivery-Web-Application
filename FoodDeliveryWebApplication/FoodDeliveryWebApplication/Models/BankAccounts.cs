using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class BankAccounts:BankIdModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int AccId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Bank")]
        public Nullable<int> fk_BankName { get; set; }
        [Required(ErrorMessage = "Required!!")]
        public string Branch { get; set; }
        [Required(ErrorMessage ="Required!!")]
        [DisplayName("Account Number")]
        [RegularExpression(@"^\d{9,18}$",ErrorMessage ="Invalid account number")]
        public string AccNumber { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("IFSC Code")]
        [RegularExpression("^[A-Za-z]{4}[a-zA-Z0-9]{7}$",ErrorMessage ="invalid ifsc code")]
        public string IfscCode { get; set; }
      
        public Nullable<int> rder_fk_CusId { get; set; }
        public string UserEmailId { get; set; }
        public string PinNumber { get; set; }
        public Nullable<int> roleId { get; set; }



    }
}