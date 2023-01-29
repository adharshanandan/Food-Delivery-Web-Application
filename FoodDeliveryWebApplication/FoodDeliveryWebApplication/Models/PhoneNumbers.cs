using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class PhoneNumbers
    {
        [Key]
        public int PhoneId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }
        public int?  Phn_fk_CusId { get; set; }
    }
}