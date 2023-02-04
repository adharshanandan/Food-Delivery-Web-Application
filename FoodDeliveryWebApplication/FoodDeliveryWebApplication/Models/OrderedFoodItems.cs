using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FoodDeliveryWebApplication.Models
{
    public class OrderedFoodItems:CustomerAddresses
    {
        [Key]
        public int id { get; set; }
        public Nullable<int> fk_OrderId{ get; set; }
        [DisplayName("Food Detais")]
        public Nullable<int> fk_DishId { get; set; }
        public Nullable<int> DishQuantity { get; set; }
    }
}