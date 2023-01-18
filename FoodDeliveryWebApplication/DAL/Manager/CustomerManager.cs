using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class CustomerManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public List<tbl_Customer> GetUserDetails()
        {
            return db.tbl_Customer.Where(e => e.CusStatus == "A").ToList();
        }
        public string InsertCustomer(tbl_Customer insObj)
        {
            tbl_Login isEmailExist = db.tbl_Login.Where(e => e.UserId == insObj.CusEmail && e.UserRole == insObj.CusRole).SingleOrDefault();
            if (isEmailExist != null)
            {
                return "Exist";
            }
            else
            {
                db.tbl_Customer.Add(insObj);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }

            }

        }
        
    }
}
