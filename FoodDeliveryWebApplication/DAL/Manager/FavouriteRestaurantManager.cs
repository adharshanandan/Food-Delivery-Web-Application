using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class FavouriteRestaurantManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public string DeleteFromFavList(int restId, string cusEmail)
        {
            tbl_FavRestaurants remObj = db.tbl_FavRestaurants.Where(e => e.tbl_Customer.CusEmail == cusEmail && e.Fav_fk_RestId == restId).FirstOrDefault();
            db.tbl_FavRestaurants.Remove(remObj);
            int status = db.SaveChanges();
            if (status > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }

        }

        public string AddToFavList(tbl_FavRestaurants insObj)
        {
            db.tbl_FavRestaurants.Add(insObj);
            int status = db.SaveChanges();
            if (status > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }
        public List<tbl_Restaurant> GetFavListByCusId(string cusEmailId)
        {
            List<tbl_FavRestaurants> favList= db.tbl_FavRestaurants.Where(e => e.tbl_Customer.CusEmail == cusEmailId).ToList();
            List<tbl_Restaurant> restList = new List<tbl_Restaurant>();
            foreach(var item in favList)
            {
                restList.Add(db.tbl_Restaurant.Where(e => e.RestId == item.Fav_fk_RestId).FirstOrDefault());
            }
            return restList;
        }

        public bool IsFavouriteRestaurant(int restId,string cusEmail)
        {
            return db.tbl_FavRestaurants.Where(e => e.Fav_fk_RestId == restId && e.tbl_Customer.CusEmail == cusEmail).Any() ? true : false;

        }
    }

}
