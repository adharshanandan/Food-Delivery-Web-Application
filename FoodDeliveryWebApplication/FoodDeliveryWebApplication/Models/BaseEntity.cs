using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class BaseEntity
    {
       

        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Email Id"),EmailAddress]
        public string EmailId { get; set; }
        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$")]
        public string Password { get; set; }

    }
}