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


namespace FoodDeliveryWebApplication.Controllers
{
    public class CustomerController : Controller
    {
        CustomerManager cusMngr = new CustomerManager();
        // GET: Customer
        public ActionResult Index()
        {
            
            List<User> userList = new List<User>();
            var returnList = cusMngr.GetUserDetails();
            foreach(var item in returnList)
            {
                userList.Add(new User
                {
                    Id=item.CusId,
                    Name=item.CusName,
                    EmailId=item.CusEmail


                });
            }
            return View(userList);
            
        }
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
            if (ModelState.IsValid)
            {
                tbl_Customer insObj = new tbl_Customer();
                insObj.CusName = obj.Name;
                insObj.CusEmail = obj.EmailId;
                insObj.CusPassword = obj.Password;
                insObj.CusImage = obj.ImgUrl.FileName;
                insObj.CusPincode = obj.Pincode;
                insObj.CusStatus = "A";
                string result=cusMngr.InsertCustomer(insObj);
                if (result == "Success")
                {
                    return RedirectToAction("Index");
                }
                else if (result == "Failed")
                {
                    return View();
                }
                else if (result == "Exist")
                {
                    ViewBag.exist = "Email Already Exists";
                    return View();
                }
                else
                {
                    return View();
                }
            }
            return View();

        }
    }
}