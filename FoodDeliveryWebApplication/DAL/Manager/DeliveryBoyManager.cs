using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;

namespace DAL.Manager
{
    public class DeliveryBoyManager
    {

        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-SRG4EAKH;Initial Catalog=db_FoodOrderingApplication;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
        public string InsertDelGuy(tbl_DeliveryStaffs insObj)
        {

            db.tbl_DeliveryStaffs.Add(insObj);
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
        public tbl_DeliveryStaffs IsExistEmail(tbl_DeliveryStaffs checkObj)
        {
            return db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == checkObj.StaffEmail).SingleOrDefault();
        }

        public tbl_DeliveryStaffs IsExistPhone(tbl_DeliveryStaffs checkObj)
        {
            return db.tbl_DeliveryStaffs.Where(e => e.StaffPhone == checkObj.StaffPhone).SingleOrDefault();
        }

        public List<tbl_OrderDetails> GetPendingOrderRequestsBylocation(string emailId)
        {
            tbl_DeliveryStaffs retObj = db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == emailId).SingleOrDefault();

            return db.tbl_OrderDetails.Where(e => e.IsOrderConfirmed == "Confirmed" && e.tbl_Restaurant.RestArea == retObj.StaffArea && e.Order_fk_StaffId == null && e.tbl_DeliveryStaffs.Isfree=="Yes").ToList();
        }

        public string AcceptOrderRequest(int? id, string emailId)
        {

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            tbl_DeliveryStaffs checkObj = db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == emailId && e.Isfree == "Yes").SingleOrDefault();
            if (checkObj == null)
            {
                return "Not free";
            }
            else
            {
                SqlCommand cmd = new SqlCommand("sp_AcceptOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@orderId", id));
                cmd.Parameters.Add(new SqlParameter("@staffemail", emailId));

                string result = cmd.ExecuteScalar().ToString();
                con.Close();
                return result;

            }
        

        }

        public string OrderPickStatusChange(int id)
        {
            tbl_OrderDetails updObj = db.tbl_OrderDetails.Where(e => e.OrderId == id && e.IsPicked == "Not Picked" && e.IsOrderConfirmed == "Confirmed").SingleOrDefault();
            if (updObj == null)
            {
                return "Not found";
            }
            updObj.IsPicked = "Picked";
            db.Entry(updObj).State = EntityState.Modified;
            int status = db.SaveChanges();
            if(status>0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }




        public int GetRequestsCount(string emailId)
        {
            tbl_DeliveryStaffs retObj = db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == emailId).SingleOrDefault();

            return db.tbl_OrderDetails.Where(e => e.IsOrderConfirmed == "Confirmed" && e.tbl_Restaurant.RestArea == retObj.StaffArea && e.Order_fk_StaffId == null && e.tbl_DeliveryStaffs.Isfree == "Yes").Count();
        }

        public List<tbl_OrderDetails> GetAcceptedOrders(string emailId)
        {
          
            return db.tbl_OrderDetails.Where(e => e.IsOrderConfirmed == "Confirmed" && e.tbl_DeliveryStaffs.StaffEmail==emailId && e.IsDelivered=="N").ToList();
        }

        public List<tbl_OrderDetails> GetDeliveredOrders(string emailId)
        {
            return db.tbl_OrderDetails.Where(e => e.IsDelivered == "Y" && e.tbl_DeliveryStaffs.StaffEmail == emailId).ToList();
        }

        public tbl_OrderDetails GetOrderById(int? id)
        {
            return db.tbl_OrderDetails.Find(id);
        }
        public int ConfirmOrder(int? id)
        {
            tbl_OrderDetails updObj = db.tbl_OrderDetails.Find(id);
            updObj.IsDelivered = "Y";
            if (updObj.PaymentMode == "COD")
            {
                updObj.IsPaid = "Y";
            }
            
            db.Entry(updObj).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public List<tbl_DeliveryStaffs> GetDelBoysListToApprove()
        {
            return db.tbl_DeliveryStaffs.Where(e => e.IsValid == "N").ToList();
        }

        public int ApproveDelboys(int? id)
        {
            tbl_DeliveryStaffs updObj = db.tbl_DeliveryStaffs.Where(e => e.StaffId == id).SingleOrDefault();
            updObj.IsValid = "Yes";
            db.Entry(updObj).State = EntityState.Modified;
            return db.SaveChanges();
        }

        

    }
}
