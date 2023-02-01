using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class LoginManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public int LoginUser(tbl_Login checkObj)
        {
            tbl_Login isExist = db.tbl_Login.Where(e => e.UserId == checkObj.UserId && e.UserPassword == checkObj.UserPassword).SingleOrDefault();
            int roleId = 0;
            if (isExist != null)
            {
                roleId = Convert.ToInt32(isExist.UserRole);
                return roleId;
            }
            return roleId;
        }
        public tbl_DeliveryStaffs GetStaffDetailsByEmail(string emailId)
        {
            return db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == emailId && e.IsValid == "Yes" && e.StaffAccStatus == "A").SingleOrDefault();
        }

        public tbl_Restaurant GetRestDetailsByEmail(string emailId)
        {
            return db.tbl_Restaurant.Where(e => e.RestEmail == emailId).SingleOrDefault();
        }
        public tbl_Customer GetCustomerDetailsByEmail(string emailId)
        {

            return db.tbl_Customer.Where(x => x.CusEmail == emailId).FirstOrDefault();

        }

    }
}
