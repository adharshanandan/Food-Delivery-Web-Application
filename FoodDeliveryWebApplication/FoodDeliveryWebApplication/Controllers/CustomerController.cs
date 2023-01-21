using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Manager;
using FoodDeliveryWebApplication.Models;
using DAL.Models;
using AutoMapper;
using System.Net;
using System.IO;
using System.Web.Hosting;



namespace FoodDeliveryWebApplication.Controllers
{
    public class CustomerController : Controller
    {
        CustomerManager cusMngr = new CustomerManager();
        EmailVerification emailVer = new EmailVerification();
        // GET: Customer
        //public ActionResult Index()
        //{
            
        //    List<User> userList = new List<User>();
        //    var returnList = cusMngr.GetUserDetails();
        //    foreach(var item in returnList)
        //    {
        //        userList.Add(new User
        //        {
        //            Id=item.CusId,
        //            Name=item.CusName,
        //            EmailId=item.CusEmail


        //        });
        //    }
        //    return View(userList);
            
        //}
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User obj)
        {
          
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Customer insObj = new tbl_Customer();
            insObj.CusEmail = obj.EmailId;
            var isExist = cusMngr.IsExist(insObj);
            if (isExist != null)
            {
                ModelState.AddModelError("EmailId", "Email already exists");
                
            }


            if (ModelState.IsValid)
            {
                
                insObj.CusName = obj.Name;               
                insObj.CusPassword = obj.Password;
                insObj.CusImage = obj.ImgUrl.FileName;
                string savePath = Server.MapPath("~/Content/CustomerProfilePictures");
                string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
                obj.ImgUrl.SaveAs(saveThumbImagePath);
                insObj.CusPincode = obj.Pincode;
                insObj.CusStatus = "A";
                insObj.IsValid = "No";
                insObj.CusRole = 2;

                string result = cusMngr.InsertCustomer(insObj);
                if (result == "Success")
                {
                    int regId = Convert.ToInt32(cusMngr.GetCustomerIdByEmailId(obj.EmailId));
                    BuildEmailTemplate(regId);
                    TempData["CusSuccess"] = "Registered successfully. Please check your Email to activate your account";
                    return RedirectToAction("Login","Home");
                }
                else if (result == "Failed")
                {
                    return View();
                }
                else if (result == "Exist")
                {
                    ViewBag.exist = "Email already exists. Please login !";
                    return View();
                }
                else
                {
                    return View();
                }

            }
            return View();

        }
        // Method to create Email content template
        public void BuildEmailTemplate(int regId)
        {
           
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            string sendTo = cusMngr.GetCustomerEmailById(regId);
            var url = "https://localhost:44329/" + "Customer/Confirm?regId=" + regId;
            body = body.Replace("@ViewBag.confirmationLink", url);
            body = body.ToString();
            TempData["EmailStatus"] = emailVer.SendEmail("Your account is created successfully", body, sendTo);

        }
        //Action method to return confirm view
        public ActionResult Confirm(int? regId)
        {
            ViewBag.regId = regId;
            return View();
        }
        //Method to pass Id value to update isValid field of customer to "Yes"
        public JsonResult RegisterConfirm(int regId)
        {
            var msg = cusMngr.ActivateAccount(regId);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}