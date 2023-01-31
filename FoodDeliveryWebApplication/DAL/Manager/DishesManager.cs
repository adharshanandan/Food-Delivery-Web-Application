using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class DishesManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public List<tbl_Dishes> GetDishesDetails(string search, string emailId)
        {
            if (search != null)
            {
                return db.tbl_Dishes.Where(e => e.DishName.Contains(search) || e.DishDesc.Contains(search) || e.VegOrNonveg.Contains(search) || e.tbl_Category.CatName.Contains(search) && e.tbl_Restaurant.RestEmail == emailId).ToList();
            }
            else
            {
                return db.tbl_Dishes.Where(e => e.tbl_Restaurant.RestEmail == emailId).ToList();
            }
        }

        public List<tbl_Category> GetCategoryList()
        {
            return db.tbl_Category.Where(e => e.CatStatus == "A").ToList();
        }
        public tbl_Category GetCategoryById(int id)
        {
            return db.tbl_Category.Where(e => e.CatStatus == "A" && e.CatId == id).SingleOrDefault();
        }
        public List<tbl_Offers> GetOffersList()
        {
            return db.tbl_Offers.ToList();
        }
        public tbl_Category GetOffersById(int id)
        {
            return db.tbl_Category.Where(e => e.CatId == id).SingleOrDefault();
        }
        public tbl_Dishes GetDishById(int id)
        {
            return db.tbl_Dishes.Where(e => e.DishId == id).SingleOrDefault();
        }
        public  int isExist(tbl_Dishes checkObj, string emailId)
        {
            return db.tbl_Dishes.Where(e => e.DishName == checkObj.DishName && e.Dish_fk_Cat == checkObj.Dish_fk_Cat && e.tbl_Restaurant.RestEmail == emailId).Count();
        }
        
        public string DishInsert(tbl_Dishes insObj,string emailId)
        {
            var query = from p in db.tbl_Restaurant where p.RestEmail == emailId select p.RestId;
            insObj.Dish_fk_Rest = Convert.ToInt32(query.Single());
            tbl_Dishes checkObj = db.tbl_Dishes.Where(e => e.DishName == insObj.DishName && e.Dish_fk_Cat == insObj.Dish_fk_Cat&&e.Dish_fk_Rest==insObj.Dish_fk_Rest).SingleOrDefault();
            if (checkObj != null)
            {
                return "Exist";
            }
            db.tbl_Dishes.Add(insObj);
            int status=db.SaveChanges();
            if (status > 0)
            {
                return "Success";

            }
            else
            {
                return "Failed";
            }
            
        }
        public string DishUpdate(tbl_Dishes insObj,string emailId)
        {
            var query = from p in db.tbl_Restaurant where p.RestEmail == emailId select p.RestId;
            insObj.Dish_fk_Rest = Convert.ToInt32(query.Single());
            db.Entry(insObj).State = System.Data.Entity.EntityState.Modified;
            int status = db.SaveChanges();
            if (status > 0)
            {
                return "Updated";
            }
            else
            {
                return "Failed";
            }
        }
        public string DeleteDishById(int id)
        {
            tbl_Dishes remObj = db.tbl_Dishes.Find(id);
            try
            {
                db.tbl_Dishes.Remove(remObj);
                int status = db.SaveChanges();
                if (status > 0)
                {
                    return "Deleted";
                }
                else
                {
                    return "Failed";
                }
            }
            catch
            {
                return "Failed";
            }
        }

        public List<tbl_Dishes> DishesForDisplay(string search, int id,string vegId,int? catId)
        {
            StringBuilder query = new StringBuilder();
            string prefix = "Select * from tbl_Dishes where Dish_fk_Rest="+id+"";
            query.Append(prefix);
            prefix = " and";
            if (search != "")
            {                
                query.Append(prefix +" DishName Like '"+search+"%' ");
                
            }

            if (vegId != "")
            {
                query.Append(prefix + " VegOrNonveg='" + vegId + "'");
                
            }
            if (catId != 0)
            {
                query.Append(prefix + " Dish_fk_Cat='" + catId + "'");
            }
            return db.tbl_Dishes.SqlQuery(query.ToString()).ToList();
          
        }


    }
}
