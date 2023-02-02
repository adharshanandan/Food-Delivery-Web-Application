using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryWebApplication.Models
{
    public class ReviewRating
    {
        [Key]
        public int RevId { get; set; }
        [DisplayName("Review")]
        [Required]
        public string ReviewContent { get; set; }
        [Required]
        public Nullable<int> Rating { get; set; }
        public Nullable<int> Rev_fk_CusId { get; set; }
        public Nullable<int> Rev_fk_RestId { get; set; }
        public string RestName { get; set; }
        public string RestImage { get; set; }
        public DateTime PostedDate { get; set; }
        public int RatingAverage { get; set; }
        public int ReviewCount { get; set; }
        public string CusName { get; set; }
    }
}