using DAL.Manager;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodDeliveryWebApplication.Models;

namespace FoodDeliveryWebApplication.Controllers
{
    public class LoginController : Controller
    {
        CustomerManager cusMngr = new CustomerManager();
        RestaurantManager restMngr = new RestaurantManager();
        DeliveryBoyManager delMngr = new DeliveryBoyManager();
        LoginManager loginMngr = new LoginManager();
        EmailVerification sendEmail = new EmailVerification();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login obj)
        {
            tbl_Login loginObj = new tbl_Login();
            loginObj.UserId = obj.UserEmailId;
            loginObj.UserPassword = obj.UserPassword;
            int roleId = loginMngr.LoginUser(loginObj);

            if (roleId == 1)
            {
                Session["Admin"] = obj.UserEmailId;
                Session["WishAdmin"] = "Hi, Admin";

                return RedirectToAction("CategoryList", "Admin");

            }
            else if (roleId == 2)
            {
                Session["Customer"] = obj.UserEmailId;
                tbl_Customer cusObj = loginMngr.GetCustomerDetailsByEmail(obj.UserEmailId.ToString());
                if (cusObj.IsValid != "Yes")
                {
                    ViewBag.validCheckCus = "Email is not verfied";
                    return View();
                }
                if (cusObj.CusStatus != "A")
                {
                    ViewBag.statusCheckCus = "Account not found";
                    return View();
                }
                Session["CustomerDetailsOnLayout"] = new string[] { cusObj.CusImage,cusObj.CusName};
                return RedirectToAction("FoodItems", "Customer");


            }
            else if (roleId == 3)
            {
                Session["Restaurant"] = obj.UserEmailId;
                tbl_Restaurant restObj = loginMngr.GetRestDetailsByEmail(obj.UserEmailId);
                if (restObj.IsValid != "Yes")
                {
                    ViewBag.validCheckRest = "Account is not approved by admin. Please wait..";
                    return View();
                }
                if (restObj.RestStatus != "A")
                {
                    ViewBag.statusCheckRest = "Account not found";
                    return View();
                }
                Session["RestDetailsOnLayout"] = new string[] { restObj.RestImage, restObj.RestName };
                return RedirectToAction("DishList", "Dishes");
                
            }
            else if (roleId == 4)
            {
                Session["DeliveryBoy"] = obj.UserEmailId;
                tbl_DeliveryStaffs staffObj = loginMngr.GetStaffDetailsByEmail(obj.UserEmailId);
                if (staffObj.IsValid != "Yes")
                {
                    ViewBag.validCheckRest = "Account is not approved by admin. Please wait..";
                    return View();
                }
                if (staffObj.StaffAccStatus != "A")
                {
                    ViewBag.statusCheckRest = "Account not found";
                    return View();
                }
                Session["StaffDetailsOnLayout"] = new string[] { staffObj.staffImage, staffObj.StaffName };
                return RedirectToAction("PendingOrderRequests", "DeliveryBoy");
            }
            else
            {
                ViewBag.msg = "User not found";
                return View();
            }

        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(BaseEntity obj)
        {
            if (obj == null)
            {
                return Json("Bad request", JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                tbl_Login insObj = new tbl_Login();
                insObj.UserPassword = obj.Password;
                insObj.UserId = obj.EmailId;
                string result = loginMngr.InsertNewPassword(insObj);
                if (result == "Success")
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed to update", JsonRequestBehavior.AllowGet);

                }
            }

            return Json("Invalid input", JsonRequestBehavior.AllowGet);






        }

        public JsonResult IsExistEmail(string emailId)
        {
            if (emailId == "")
            {
                return Json("Please fill the field",JsonRequestBehavior.AllowGet);
            }
            string result = loginMngr.IsEmailExist(emailId);
            if(result=="Not found")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (result == "Found")
            {
                Random RandNo = new Random();
                string otp = RandNo.Next(100001, 999999).ToString();
                Session["PswdResetOtp"] = otp;
                string status = sendEmail.SendEmail("Reset Password", "Your one time password is : " + otp, emailId);               
                return Json("Found", JsonRequestBehavior.AllowGet);
            }
            return Json("Error occured", JsonRequestBehavior.AllowGet);

        }

        public JsonResult OtpMatchCheck(string otp)
        {
            if (otp =="")
            {
                return Json("Please fill the field", JsonRequestBehavior.AllowGet);
            }
            if (otp == Session["PswdResetOtp"].ToString())
            {
                return Json("Matched", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(" Not Matched", JsonRequestBehavior.AllowGet);
            }
        
        }
    }
}