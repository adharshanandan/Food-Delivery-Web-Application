using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class LoginManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public int LoginUser(tbl_Login checkObj)
        {
            tbl_Login isExist = db.tbl_Login.Where(e => e.UserId == checkObj.UserId && e.UserPassword == checkObj.UserPassword).SingleOrDefault();
            int roleId = 0;
            if (isExist != null)
            {
                roleId = Convert.ToInt32(isExist.UserRole);
                return roleId;
            }
            return roleId;
        }
        public tbl_DeliveryStaffs GetStaffDetailsByEmail(string emailId)
        {
            return db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == emailId && e.IsValid == "Yes" && e.StaffAccStatus == "A").SingleOrDefault();
        }

        public tbl_Restaurant GetRestDetailsByEmail(string emailId)
        {
            return db.tbl_Restaurant.Where(e => e.RestEmail == emailId).SingleOrDefault();
        }
        public tbl_Customer GetCustomerDetailsByEmail(string emailId)
        {

            return db.tbl_Customer.Where(x => x.CusEmail == emailId).FirstOrDefault();

        }

        public string IsEmailExist(string emailId)
        {
            tbl_Login checkObj = db.tbl_Login.Where(e => e.UserId == emailId).SingleOrDefault();
            if (checkObj == null)
            {
                return "Not found";
            }
            else
            {
               return "Found";
            }
        }

        public string InsertNewPassword(tbl_Login updObj)
        {
            tbl_Login loginObj= db.tbl_Login.Where(e => e.UserId == updObj.UserId).SingleOrDefault();
            int status;
            if (loginObj.UserRole == 1)
            {
                tbl_Admin rstPswdAdmObj = db.tbl_Admin.Where(e => e.AdmEmail == loginObj.UserId).SingleOrDefault();
                rstPswdAdmObj.AdmPassword = updObj.UserPassword;
                db.Entry(rstPswdAdmObj).State = System.Data.Entity.EntityState.Modified;
                status = db.SaveChanges();
                if (status > 0)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            if (loginObj.UserRole == 2)
            {
                tbl_Customer rstPswdCusObj = db.tbl_Customer.Where(e => e.CusEmail == loginObj.UserId).SingleOrDefault();
                rstPswdCusObj.CusPassword = updObj.UserPassword;
                db.Entry(rstPswdCusObj).State = System.Data.Entity.EntityState.Modified;
                status = db.SaveChanges();
                if (status > 0)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            if (loginObj.UserRole == 3)
            {
                tbl_Restaurant rstPswdRestObj = db.tbl_Restaurant.Where(e => e.RestEmail == loginObj.UserId).SingleOrDefault();
                rstPswdRestObj.RestPassword = updObj.UserPassword;
                db.Entry(rstPswdRestObj).State = System.Data.Entity.EntityState.Modified;
                status = db.SaveChanges();
                if (status > 0)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }


            }
            if (loginObj.UserRole == 4)
            {
                tbl_DeliveryStaffs rstPswdDelObj = db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == loginObj.UserId).SingleOrDefault();
                rstPswdDelObj.StaffPassword = updObj.UserPassword;
                db.Entry(rstPswdDelObj).State = System.Data.Entity.EntityState.Modified;
                status = db.SaveChanges();
                if (status > 0)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }

            }
            return "Not found";

        }

    }
}
