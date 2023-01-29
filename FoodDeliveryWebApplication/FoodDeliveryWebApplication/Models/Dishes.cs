using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryWebApplication.Models;
using static FoodDeliveryWebApplication.Models.User;

namespace FoodDeliveryWebApplication.Models
{
    public class Dishes
    {
        [Key]
        [ScaffoldColumn(false)]
        public int DishId { get; set; } 
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Name")]
        public string DishName { get; set; }
       
        
        [DisplayName("Photo")]
        public string DishImage { get; set; }
        [Required(ErrorMessage = "Required!!")]

        [DisplayName("Photo")]
        [ValidateFileAttribute]
        public HttpPostedFileBase DishImgUrl { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Price")]
        public string DishPrice { get; set; }
        [DisplayName("Description")]
        public string DishDesc { get; set; } 
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Veg/Non-Veg")]
        public string VegorNonveg { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "Required!!")]
        public int DishCategoryId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Offer")]

        
        public int DishRestaurantId { get; set; }
    
        public string DishCategory { get; set; }
     
        public string buttonName { get; set; }

    }
}