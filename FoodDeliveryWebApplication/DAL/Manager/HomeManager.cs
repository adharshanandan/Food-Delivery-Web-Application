using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;


namespace DAL.Manager
{
    public class HomeManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public string InsertEnquiry(tbl_ContactUs insObj)
        {
            db.tbl_ContactUs.Add(insObj);
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
        public int LoginUser(tbl_Login checkObj)
        {
            tbl_Login isExist = db.tbl_Login.Where(e => e.UserId == checkObj.UserId && e.UserPassword == checkObj.UserPassword).SingleOrDefault();
            int roleId=0;
            if (isExist != null)
            {
                roleId = Convert.ToInt32(isExist.UserRole);
                return roleId;
            }
            return roleId;
        }
    }
}
