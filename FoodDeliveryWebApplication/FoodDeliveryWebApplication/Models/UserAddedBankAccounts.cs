using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDeliveryWebApplication.Models
{
    public class UserAddedBankAccounts: BankAccounts
    {
        public Nullable<int> fk_UserId { get; set; }
    }
}