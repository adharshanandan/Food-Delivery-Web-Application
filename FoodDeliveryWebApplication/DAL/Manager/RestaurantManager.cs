using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class RestaurantManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public List<tbl_Restaurant> GetRestaurantDetails()
        {
            return db.tbl_Restaurant.Where(e => e.RestStatus == "A").ToList();
        }
        public string InsertRestaurant(tbl_Restaurant insObj)
        {

            db.tbl_Restaurant.Add(insObj);
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
        public tbl_Restaurant IsExistEmail(tbl_Restaurant checkObj)
        {
            return db.tbl_Restaurant.Where(e => e.RestEmail == checkObj.RestEmail).SingleOrDefault();
        }

        public tbl_Restaurant IsExistPhone(tbl_Restaurant checkObj)
        {
            return db.tbl_Restaurant.Where(e => e.RestPhone == checkObj.RestPhone).SingleOrDefault();
        }
    }
}
