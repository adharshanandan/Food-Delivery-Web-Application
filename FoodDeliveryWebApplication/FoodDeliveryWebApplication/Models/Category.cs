using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static FoodDeliveryWebApplication.Models.User;

namespace FoodDeliveryWebApplication.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Food Category")]
        public string CatName { get; set; }
        [DisplayName("Photo")]
        public string CatImage { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Photo")]
        [ValidateFileAttribute]
        public HttpPostedFileBase CatImgUrl { get; set; }
        [DisplayName("Status")]
        public string CatStatus { get; set; }
    }
}