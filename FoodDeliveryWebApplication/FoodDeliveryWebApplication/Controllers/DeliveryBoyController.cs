using FoodDeliveryWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DAL.Models;
using DAL.Manager;

namespace FoodDeliveryWebApplication.Controllers
{
    public class DeliveryBoyController : Controller
    {
        // GET: DeliveryBoy
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(DeliveryGuy obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DeliveryGuy delObj = obj;
            if (Session["location"] != null)
            {
                List<PostOffice> postList = (List<PostOffice>)Session["location"];

                foreach (var item in postList)
                {
                    if (item.Name == obj.StaffArea)
                    {
                        delObj.StaffCountry = item.Country;
                        delObj.StaffDist = item.District;
                        delObj.StaffState = item.State;
                    }
                }

            }
            DeliveryBoyManager delMngr = new DeliveryBoyManager();
            tbl_DeliveryStaffs insObj = new tbl_DeliveryStaffs();

            //Here first set values for properties to check if they already exists in the database
            insObj.StaffEmail = obj.EmailId;
            insObj.StaffPhone = obj.PhoneNo;

            var isExistEmail = delMngr.IsExistEmail(insObj);
            if (isExistEmail != null)
            {
                ModelState.AddModelError("EmailId", "Email already exists");
                return View();
            }
            var isExistPhone = delMngr.IsExistPhone(insObj);
            if (isExistPhone != null)
            {
                ModelState.AddModelError("PhoneNo", "Phone number is already registered");
            }
            if (ModelState.IsValid)
            {
                insObj.StaffName = obj.Name;            
                insObj.StaffDob = Convert.ToDateTime(obj.Dob);
                insObj.staffImage = obj.ImgUrl.FileName;
                string savePath = Server.MapPath("~/Content/DelGuyProfilePictures");
                string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
                obj.ImgUrl.SaveAs(saveThumbImagePath);
                insObj.JoinDate = DateTime.Now;
                insObj.Gender = obj.Gender;
                insObj.VehicleNo = obj.VehicleNo;
                insObj.VehicleType = obj.VehicleType;
                insObj.DrivingLicense = obj.DrivingLicense;
                insObj.AdhaarNo = obj.AdhaarNo;
                insObj.StaffAccStatus = "A";
                insObj.Isfree = "Yes";
                insObj.IsValid = "Yes";
                insObj.StaffRole = 4;
                insObj.StaffArea = obj.StaffArea;
                insObj.StaffPassword = obj.Password;
                insObj.StaffCountry = delObj.StaffCountry;
                insObj.StaffState = delObj.StaffState;
                insObj.StaffDistrict = delObj.StaffDist;
                string result = delMngr.InsertDelGuy(insObj);
                if (result == "Success")
                {
                    TempData["DelGuySuccess"] = "Registered successfully. Please Login";
                    return RedirectToAction("Login", "Home");
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
                    catch (Exception)
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
    }
}