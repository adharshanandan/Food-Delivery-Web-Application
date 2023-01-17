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
        [ScaffoldColumn(false)]
        public int ContId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string EmailId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}