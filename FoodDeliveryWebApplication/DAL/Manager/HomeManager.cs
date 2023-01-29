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
       
    }
}
