using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class AddressType
    {
        [Key]
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
    }
}