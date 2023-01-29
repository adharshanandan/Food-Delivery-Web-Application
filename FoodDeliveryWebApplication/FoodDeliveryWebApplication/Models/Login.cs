using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Required!!")]
        [EmailAddress(ErrorMessage ="Invalid Email id")]
        [DisplayName("Username")]
        public string UserEmailId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Password")]
        public string UserPassword { get; set; }
    }
}