using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Data.Entity;
namespace DAL.Manager
{
    public class CategoryManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public List<tbl_Category> GetCategoryList()
        {
            return db.tbl_Category.ToList();
        }
        public tbl_Category GetCatById(int id)
        {
            return db.tbl_Category.Find(id);
        }

        public string InsertCategory(tbl_Category insObj)
        {
            db.tbl_Category.Add(insObj);
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
        public string UpdateCategory(tbl_Category UpdateObj)
        {
            db.Entry(UpdateObj).State = EntityState.Modified;
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
        public string DeleteCatById(int id)
        {
            tbl_Category remObj = db.tbl_Category.Find(id);
            db.tbl_Category.Remove(remObj);
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
    }
}
