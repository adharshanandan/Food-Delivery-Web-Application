using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using FoodDeliveryWebApplication.Models;
using System.Web.Script.Serialization;
using DAL.Models;
using DAL.Manager;


namespace FoodDeliveryWebApplication.Controllers
{
    public class RestaurantController : Controller
    {
       

        // GET: Restaurant/Create
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Restaurant obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Restaurant restObj = obj;
            if (Session["location"] != null)
            {
                List<PostOffice> postList = (List<PostOffice>)Session["location"];

                foreach (var item in postList)
                {
                    if (item.Name == obj.RestArea)
                    {
                        restObj.RestCountry = item.Country;
                        restObj.RestDistrict = item.District;
                        restObj.RestState = item.State;
                    }
                }

            }
            RestaurantManager restMngr = new RestaurantManager();
            tbl_Restaurant insObj = new tbl_Restaurant();
            insObj.RestName = obj.Name;
            insObj.RestPhone = obj.RestPhone;
            insObj.RestEmail = obj.EmailId;
            insObj.RestPincode = obj.Pincode;
            insObj.RestImage = obj.ImgUrl.FileName;
            string savePath = Server.MapPath("~/Content/RestaurantProfilePictures");
            string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
            obj.ImgUrl.SaveAs(saveThumbImagePath);
            insObj.RestCountry = restObj.RestCountry;
            insObj.RestState = restObj.RestState;
            insObj.RestDistrict = restObj.RestDistrict;
            insObj.RestPassword = obj.Password;
            insObj.RestStatus = "A";
            insObj.RestTradeLicense = obj.RestTradeLicense;
            insObj.RestRole = 3;
            insObj.RestArea = obj.RestArea;
            insObj.IsValid = "Yes";
            var isExistEmail = restMngr.IsExistEmail(insObj);
            if (isExistEmail != null)
            {
                ModelState.AddModelError("EmailId", "Email already exists");
                return View();
            }
            var isExistPhone = restMngr.IsExistPhone(insObj);
            if (isExistPhone != null)
            {
                ModelState.AddModelError("RestPhone", "Phone number is already registered");
            }
            if (ModelState.IsValid)
            {

                
                string result = restMngr.InsertRestaurant(insObj);
                if (result == "Success")
                {
                    TempData["RestSuccess"] = "Registered successfully. Please Login";
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
              
        public ActionResult GetLocation(string pincode)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://api.postalpincode.in/");
            var resTask = hc.GetAsync($"pincode/{pincode}");
            resTask.Wait();
            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                
                var readTask = result.Content.ReadAsStringAsync();             
                readTask.Wait();
         
                var locations = readTask.Result;
                if (locations != null)
                {
                    var serializer = new JavaScriptSerializer();
                    var _list = serializer.Deserialize<PincodeProps[]>(locations);                   
                    List<PostOffice> pinCodeList = new List<PostOffice>();
                    try
                    {
                        foreach (var lst in _list)
                        {
                            if (lst.PostOffice != null)
                            {
                                foreach (var pcl in lst.PostOffice)
                                {
                                    pinCodeList.Add(new PostOffice
                                    {

                                        Name = pcl.Name,
                                        State = pcl.State,
                                        Country = pcl.Country,
                                        District = pcl.District


                                    });
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("Pincode", "Invalid pincode!!");
                                
                            }
                         

                        }

                    }
                    catch(Exception)
                    {
                        throw;
                    }
              
                    Session["location"] = pinCodeList;
                    return Json(pinCodeList, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "details not found please");
                }

            }
            else
            {
                
                ModelState.AddModelError(string.Empty, "invalid request");
            }
            return View();
  
        }

        // POST: Restaurant/Create


    }
}
