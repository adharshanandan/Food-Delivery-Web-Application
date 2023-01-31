using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class PaymentManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public List<tbl_UserBankAcc> GetAllBankAccountsofUser(string userEmail)
        {
            return db.tbl_UserBankAcc.Where(e =>e.tbl_Customer.CusEmail == userEmail).ToList();
        }
    }
}
