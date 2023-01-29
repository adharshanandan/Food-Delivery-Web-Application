using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static FoodDeliveryWebApplication.Models.User;

namespace FoodDeliveryWebApplication.Models
{
    public class UserEdit
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required!!")]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [DisplayName("Profile picture")]
        [ValidateFileAttribute]
        public HttpPostedFileBase ImgUrl { get; set; }
        [Required(ErrorMessage = "Required!!")]
        [RegularExpression("^[6][7-9]{1}[0-9]{4}$", ErrorMessage = "Invalid Picode")]

        public string Pincode { get; set; }

    }
}