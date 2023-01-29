using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FoodDeliveryWebApplication.Models
{
    public class Restaurant:User
    {
        public string RestCountry { get; set; }
        public string RestState { get; set; }
        public string RestDistrict { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Location")]
        public string RestArea { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Trade License")]
        public string RestTradeLicense { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Phone Number")]
        [RegularExpression("^[0-9]{10}$",ErrorMessage ="Please enter a valid phone number")]
        public string RestPhone { get; set; }
        public bool isFavourite { get; set; }
     


    }
}