using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDeliveryWebApplication.Models
{
    public class PayAmount
    {
        public decimal TotalAmount { get; set; }
        public decimal DelveryCharge { get; set; } = 30M;
        public decimal Tax { get; set; } = 43.83M;
        public decimal ToPay { get; set; } 
        public decimal offerPercentage { get; set; }
    }
}