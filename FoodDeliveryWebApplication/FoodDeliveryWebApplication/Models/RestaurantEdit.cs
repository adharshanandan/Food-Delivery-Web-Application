using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodDeliveryWebApplication.Models
{
    public class RestaurantEdit
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string RestCountry { get; set; }
        [DisplayName("State")]
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
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number")]
        public string RestPhone { get; set; }
        [Required(ErrorMessage = "Required!!")]
        public string Name { get; set; }
     
        [Required(ErrorMessage = "Required!!")]
        [RegularExpression("^[6][7-9]{1}[0-9]{4}$", ErrorMessage = "Invalid Picode")]

        public string Pincode { get; set; }
        public string Image { get; set; }
    }
}