using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace DAL.Manager
{
    public class RestaurantManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        CustomerManager cusMngr = new CustomerManager();
        public List<tbl_Restaurant> GetRestaurantDetails()
        {
            return db.tbl_Restaurant.Where(e => e.RestStatus == "A").ToList();
        }

        public List<tbl_Restaurant> GetRestaurantDetailsById(int id)
        {
            return db.tbl_Restaurant.Where(e => e.RestId == id).ToList();
        }

        public tbl_Restaurant RestaurantDetailsById(int id)
        {
            return db.tbl_Restaurant.Where(e => e.RestId == id && e.RestStatus == "A").SingleOrDefault();
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
        public tbl_Restaurant IsExistEmail(string emailId)
        {
            return db.tbl_Restaurant.Where(e => e.RestEmail == emailId).SingleOrDefault();
        }

        public tbl_Restaurant IsExistPhone(tbl_Restaurant checkObj)
        {
            return db.tbl_Restaurant.Where(e => e.RestPhone == checkObj.RestPhone).SingleOrDefault();
        }
        public int GetRestaurantId(string Emailid)
        {
            tbl_Restaurant obj = db.tbl_Restaurant.Where(e => e.RestEmail == Emailid).SingleOrDefault();
            return obj.RestId;
        }
        public List<tbl_Restaurant> GetNearestRestaurantDetails(string cusEmail)
        {
            string pinCode = cusMngr.GetCustomerPincode(cusEmail);
            return db.tbl_Restaurant.Where(e => e.RestPincode == pinCode).ToList();

        }
        public List<tbl_OrderDetails> GetAllPendingOrders(string restEmail)
        {
            return db.tbl_OrderDetails.Where(e => e.tbl_Restaurant.RestEmail == restEmail && e.IsOrderConfirmed=="N").ToList();
        }

        public string ConfirmOrderbyRest(int? id)
        {
            tbl_OrderDetails updObj = db.tbl_OrderDetails.Find(id);
            updObj.IsOrderConfirmed = "Y";
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
        public List<tbl_OrderDetails> GetOrderHistory(string restEmail)
        {
            return db.tbl_OrderDetails.Where(e => e.tbl_Restaurant.RestEmail == restEmail && e.IsDelivered == "Y").ToList();
        }
        public tbl_Restaurant RestaurantOfferDetails(int? id)
        {
            return db.tbl_Restaurant.Where(e => e.tbl_Offers.OfferPercentage != 0 && e.RestId == id).SingleOrDefault();
        }

        public List<tbl_Restaurant> GetNotApprovedRest()
        {
            return db.tbl_Restaurant.Where(e => e.IsValid == "No").ToList();
        }

        public int ApproveRestaurant(int? id)
        {
            tbl_Restaurant updObj = db.tbl_Restaurant.Where(e => e.RestId == id).SingleOrDefault();
            updObj.IsValid = "Yes";
            db.Entry(updObj).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int UpdateRestProfile(tbl_Restaurant updObj)
        {
            db.Entry(updObj).State = EntityState.Modified;
            return db.SaveChanges();
        }





    }
}
