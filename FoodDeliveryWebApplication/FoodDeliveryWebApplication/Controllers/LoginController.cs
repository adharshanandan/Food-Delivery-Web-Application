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
    }
}