using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Data.Entity;

namespace DAL.Manager
{
    public class CustomerManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public List<tbl_Customer> GetUserDetails(string emailId)
        {
            return db.tbl_Customer.Where(e => e.CusEmail == emailId).ToList();
        }
        public string InsertCustomer(tbl_Customer insObj)
        {
                db.tbl_Customer.Add(insObj);
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
        public tbl_Customer IsExist(tbl_Customer checkObj)
        {
            return db.tbl_Customer.Where(e => e.CusEmail == checkObj.CusEmail).SingleOrDefault();
        }
        public string GetCustomerEmailById(int Id)
        {
            tbl_Customer obj= db.tbl_Customer.Where(x => x.CusId == Id).FirstOrDefault();
            return obj.CusEmail.ToString();
        }
        public string ActivateAccount(int Id)
        {
            tbl_Customer updateObj= db.tbl_Customer.Where(e => e.CusId == Id).SingleOrDefault();
            updateObj.IsValid = "Yes";
            db.Entry(updateObj).State = EntityState.Modified;
            int result = db.SaveChanges();
            if (result > 0)
            {
                return "Accout activated successfully";
            }
            else
            {
                return "Error occured";
            }


        }
        public string GetCustomerIdByEmailId(string emailId)
        {
            tbl_Customer obj = db.tbl_Customer.Where(x => x.CusEmail == emailId).FirstOrDefault();
            return obj.CusId.ToString();
        }
        public tbl_Customer GetCustomerDetailsByEmailId(string emailId)
        {
            
            return db.tbl_Customer.Where(x => x.CusEmail == emailId).FirstOrDefault();
          
        }
        public string GetCustomerPincode(string cusEmail)
        {
            var pinCode = (from p in db.tbl_Customer where p.CusEmail.Contains(cusEmail) select p.CusPincode).ToArray();
            return pinCode[0].ToString();
        }

        public tbl_Customer GetCustomerDetails(string cusEmailId)
        {
            return db.tbl_Customer.Where(e => e.CusEmail == cusEmailId).SingleOrDefault();
        }

   

        public List<tbl_PhoneNumbers> GetAllPhoneNos(string cusEmailId)
        {
            return db.tbl_PhoneNumbers.Where(e => e.tbl_Customer.CusEmail == cusEmailId).ToList();
        }

        public tbl_Customer GetCustomerById(int? id)
        {
            return db.tbl_Customer.Find(id);
        }
        public string UpdateProfile(tbl_Customer updObj)
        {
            tbl_Customer obj = db.tbl_Customer.Where(e => e.CusEmail == updObj.CusEmail).SingleOrDefault();
            obj.CusName = updObj.CusName;
            obj.CusImage = updObj.CusImage;
            obj.CusPincode = updObj.CusPincode;
            db.Entry(obj).State = EntityState.Modified;
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
        public List<tbl_OrderDetails> GetAllOrdersHistoryByUserEmail(string emailId)
        {
            return db.tbl_OrderDetails.Where(e => e.tbl_Customer.CusEmail == emailId && e.IsDelivered == "Y").ToList();
        }

        public List<tbl_OrderDetails> GetAllActiveOrdersByUserEmail(string emailId)
        {
            return db.tbl_OrderDetails.Where(e => e.tbl_Customer.CusEmail == emailId && e.IsDelivered == "N").ToList();
        }

        public int InsOrderDetailsPaidCus(tbl_OrderDetails insObj)
        {
            if (insObj != null)
            {
                db.tbl_OrderDetails.Add(insObj);
                return db.SaveChanges();
            }
            return 0;
        }



    }
}
