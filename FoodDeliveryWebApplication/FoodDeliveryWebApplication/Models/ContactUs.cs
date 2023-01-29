using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class ContactUs
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ContId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        public string Message { get; set; }
    }
}