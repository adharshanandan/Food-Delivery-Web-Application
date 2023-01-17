using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodDeliveryWebApplication.Models;
using DAL.Models;
using System.Net;
using DAL.Manager;

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
        public ActionResult Home(ContactUs obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                tbl_ContactUs insObj = new tbl_ContactUs();
                insObj.ContName = obj.Name;
                insObj.ContEmail = obj.EmailId;
                insObj.ContMsg = obj.Message;
                string result = homeMngr.InsertEnquiry(insObj);
                if (result == "Success")
                {
                    ModelState.Clear();
                    ViewBag.message = "Submitted successfully";
                    return View();
                }
                else if (result == "Failed")
                {
                    ViewBag.message = "Error occured!!";
                    return View();
                }
                return View();

            }
            

            return View();
        }

       
    }
}