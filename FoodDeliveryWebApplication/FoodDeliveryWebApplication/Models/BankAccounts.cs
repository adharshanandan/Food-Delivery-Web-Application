using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDeliveryWebApplication.Models
{
    public class BankAccounts
    {
        public int AccId { get; set; }
        public Nullable<int> fk_BankName { get; set; }
        public string Branch { get; set; }
        public string AccNumber { get; set; }
        public string IfscCode { get; set; }
        public string EmailId { get; set; }
    }
}