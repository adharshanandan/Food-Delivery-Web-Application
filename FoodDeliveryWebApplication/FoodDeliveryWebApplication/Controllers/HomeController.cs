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

        //public ActionResult Home(ContactUs obj)
        //{
        //    if (obj == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        tbl_ContactUs insObj = new tbl_ContactUs();
        //        insObj.ContName = obj.Name;
        //        insObj.ContEmail = obj.EmailId;
        //        insObj.ContMsg = obj.Message;
        //        string result = homeMngr.InsertEnquiry(insObj);
        //        if (result == "Success")
        //        {
        //            ModelState.Clear();
        //            ViewBag.message = "Submitted successfully";
        //            return View();
        //        }
        //        else if (result == "Failed")
        //        {
        //            ViewBag.message = "Error occured!!";
        //            return View();
        //        }
        //        return View();

        //    }


        //    return View();
        //}
        [HttpPost]
        public async Task<JsonResult> UploadData (ContactUs obj)
        {
            string result = "";
            if (obj == null)
            {
                result = "Data not found";
                return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
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
                    result = "Submitted";
                    return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

                }
                else if (status == "Failed")
                {
                    result = "Error occured!!";
                    return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
                }
                result = "Failed";

            }
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegistrationPage()
        {
            return View();
        }
        public ActionResult CustomerRegistration()
        {
            return RedirectToAction("Create", "Customer");
        }


    }
}