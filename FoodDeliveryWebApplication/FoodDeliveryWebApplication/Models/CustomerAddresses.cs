using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class CustomerAddresses:AddressType
    {
        [Key]
        [ScaffoldColumn(false)]
        public int AddId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Door/Flat No.")]
        public string DoorOrFlatNo { get; set; }
        [Required(ErrorMessage ="Required!!")]
        [DisplayName("Landmark")]
        public string LandMark { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Address type")]
        public int? AddressType { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Pincode")]
        [RegularExpression("^[6][7-9]{1}[0-9]{4}$", ErrorMessage = "Invalid Picode")]
        public string PinCode { get; set; }
        public int? Add_fk_CusId { get; set; }
        [DisplayName("Address type")]
        public string AddressTypeName { get; set; }
    }
}