using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDeliveryWebApplication.Controllers
{
    public class LogoutController : Controller
    {
        
        public ActionResult LogoutCustomer()
        {
            if (Session["Customer"] != null)
            {

                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Login", "Login");
        }
        
        public ActionResult LogoutRestaurant()
        {
            if (Session["Restaurant"] != null)
            {

                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult LogoutAdmin()
        {
            if (Session["Admin"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Login", "Login");
        }
        public ActionResult LogoutDelBoy()
        {
            if (Session["DeliveryBoy"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Login", "Login");
        }

            
         
        
    }
}