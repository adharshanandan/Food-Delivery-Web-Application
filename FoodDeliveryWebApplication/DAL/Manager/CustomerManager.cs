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


        
    }
}
