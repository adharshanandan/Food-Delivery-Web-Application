using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class DeliveryGuy:User
    {
        [Required]
        [DisplayName("Date of birth")]
        public string Dob { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [DisplayName("Vehicle Number")]
        public string VehicleNo { get; set; }
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number")]
        [DisplayName("Phone Number")]
        public string PhoneNo { get; set; }

        [Required]
        [DisplayName("Vehicle Type")]
        public string VehicleType { get; set; }
        [Required]
        [DisplayName("Driving Licence Number")]
        public string DrivingLicense { get; set; }
        [Required]
        [DisplayName("Adhaar Id")]
        public string AdhaarNo { get; set; }
       

        public string IsFree { get; set; }
        
        public string StaffRole { get; set; }
        [Required]
        [DisplayName("Location")]
        public string StaffArea { get; set; }
        public string StaffCountry { get; set; }
        public string StaffState { get; set; }
        public string StaffDist { get; set; }
        public string IsValid { get; set; }
    }
}