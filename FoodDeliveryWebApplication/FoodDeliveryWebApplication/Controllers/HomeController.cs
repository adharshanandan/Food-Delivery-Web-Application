using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodDeliveryWebApplication.Models;
using DAL.Models;
using System.Net;
using DAL.Manager;
using System.Threading.Tasks;

namespace FoodDeliveryWebApplication.Controllers
{
    public class HomeController : Controller
    {
        HomeManager homeMngr = new HomeManager();
        

        public ActionResult Home()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult UploadData (ContactUs obj)
        {
            string result = "";
            if (obj == null)
            {
                result = "Data not found";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                tbl_ContactUs insObj = new tbl_ContactUs();
                insObj.ContName = obj.Name;
                insObj.ContEmail = obj.EmailId;
                insObj.ContMsg = obj.Message;
                string status = homeMngr.InsertEnquiry(insObj);
                if (status == "Success")
                {
                    
                    return Json("Submitted", JsonRequestBehavior.AllowGet);

                }
                else if (status == "Failed")
                {
                
                    return Json("Error occured!!", JsonRequestBehavior.AllowGet);
                }
                result = "Failed";

            }
            return Json("Please fill all the fields!!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegistrationPage()
        {
            return View();
        }
        public ActionResult CustomerRegistration()
        {
            return RedirectToAction("Create", "Customer");
        }

        public ActionResult RestaurantRegistration()
        {
            return RedirectToAction("Create", "Restaurant");
        }
        public ActionResult DeliveryGuyRegistration()
        {
            return RedirectToAction("Create", "DeliveryBoy");
        }

        
        





    }
}