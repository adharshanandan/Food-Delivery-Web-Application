using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class Cart:Dishes
    {
        [Key]
        public int CartId { get; set; }
        public DateTime? AddedDate { get; set; }
        public int? Quantity { get; set; }
        public int Cart_fk_DishId { get; set; }
        public int Cart_fk_CusId { get; set; }
        public int Cart_fk_RestId { get; set; }
        public string RestaurantName { get; set; }
        public string Image { get; set; }


    }
}