using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.Manager
{
    public class CartManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public string AddItemCart(tbl_Cart insObj)
        {
            tbl_Cart checkObj = db.tbl_Cart.Where(e => e.Cart_fk_DishId == insObj.Cart_fk_DishId && e.Cart_fk_CusId == insObj.Cart_fk_CusId && e.Cart_fk_RestId == insObj.Cart_fk_RestId).SingleOrDefault();
            if (checkObj != null)
            {
                return "Exists";
            }
            db.tbl_Cart.Add(insObj);
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

        public List<tbl_Cart> GetCartList(string emailId)
        {
            return db.tbl_Cart.Where(e => e.tbl_Customer.CusEmail == emailId).ToList();
        }

        public int GetCartItemsCount(string cusEmail)
        {
            return db.tbl_Cart.Where(e => e.tbl_Customer.CusEmail == cusEmail).Count();
        }
        public List<tbl_Cart> GetUserCartList(string cusEmail)
        {
            return db.tbl_Cart.Where(e => e.tbl_Customer.CusEmail == cusEmail).ToList();
        }
        public tbl_Cart GetCartRestDetails(string cusEmail)
        {
            return db.tbl_Cart.Where(e => e.tbl_Customer.CusEmail == cusEmail).FirstOrDefault();
        }

        public string MinusQuantity(int? id)
        {
            
            tbl_Cart updObj = db.tbl_Cart.Where(e => e.CartId == id).SingleOrDefault();
            if (updObj != null)
            {
                if (updObj.Quantity > 1)
                {
                    updObj.Quantity--;
                    db.Entry(updObj).State = EntityState.Modified;
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
                else
                {
                    return "Zero";
                }

            }
            else
            {
                return "Not found";
            }




        }

        public string PlusQuantity(int? id)
        {
            tbl_Cart updObj = db.tbl_Cart.Where(e => e.CartId == id).SingleOrDefault();
            if (updObj != null)
            {
                if (updObj.Quantity < 10)
                {
                    updObj.Quantity++;
                    db.Entry(updObj).State = EntityState.Modified;
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
                else
                {
                    return "Quantity over";
                }

            }
            else
            {
                return "Not found";
            }


        }

        public string RemoveItem(int? id)
        {
            tbl_Cart remObj = db.tbl_Cart.Where(e => e.CartId == id).SingleOrDefault();
            
            if (remObj != null)
            {
                db.tbl_Cart.Remove(remObj);
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
            else
            {
                return "Not found";
            }
        }

        public string ClearCartItems(string cusEmailId)
        {
            List<tbl_Cart> remList = db.tbl_Cart.Where(e => e.tbl_Customer.CusEmail == cusEmailId).ToList();
            if (remList != null)
            {
                foreach (var item in remList)
                {
                    db.tbl_Cart.Remove(item);
                }
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
            else
            {
                return "Empty";
            }
            
        }





    }
}
