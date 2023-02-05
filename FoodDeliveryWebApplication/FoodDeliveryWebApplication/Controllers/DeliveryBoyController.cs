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
using System.Dynamic;
using System.Collections;

namespace FoodDeliveryWebApplication.Controllers
{
    public class DeliveryBoyController : Controller
    {
        DeliveryBoyManager delMngr = new DeliveryBoyManager();
        AddressManager addMngr = new AddressManager();
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
                string savePath = Server.MapPath("~/Content/DelGuyProfilePictures");
                string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
                obj.ImgUrl.SaveAs(saveThumbImagePath);
                insObj.staffImage = "~/Content/CustomerProfilePictures/" + obj.ImgUrl.FileName;
                insObj.JoinDate = DateTime.Now;
                insObj.Gender = obj.Gender;
                insObj.VehicleNo = obj.VehicleNo;
                insObj.VehicleType = obj.VehicleType;
                insObj.DrivingLicense = obj.DrivingLicense;
                insObj.AdhaarNo = obj.AdhaarNo;
                insObj.StaffAccStatus = "A";
                insObj.Isfree = "Yes";
                insObj.IsValid = "No";
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
                    return RedirectToAction("Login", "Login");
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

        public ActionResult PendingOrderRequests()
        {
            List<tbl_OrderDetails> retList = delMngr.GetPendingOrderRequestsBylocation(Session["DeliveryBoy"].ToString());
            List<OrderDetails> disList = new List<OrderDetails>();
            if (TempData["NewRequest"] != null)
            {
                TempData["NewRequest"] = null;
            }
            if (disList.Count > 0)
            {
                TempData["NewRequest"] = retList.Count;
            }
            else
            {
                TempData["NewRequest"] = 0;
            }
            
            dynamic combinedModel = new ExpandoObject();
            
            foreach (var item in retList)
            {
                List<Cart> dishesList = new List<Cart>();
                foreach (var dish in item.tbl_OrderedFoodDetails)
                {
                    
                    dishesList.Add(new Cart
                    {
                        DishName = dish.tbl_Dishes.DishName,
                        Quantity = dish.DishQuantity,
                        DishId = Convert.ToInt32(dish.fk_DishId)

                    });
                }
                disList.Add(new OrderDetails
                {
                    fk_OrderId = item.OrderId,
                    TotalAmount = item.TotalAmount,
                    CusName = item.tbl_Customer.CusName,
                    IsOrderConfirmed = item.IsOrderConfirmed,
                    Orderdate = item.Orderdate,
                    IsDelivered = item.IsDelivered,
                    IsPicked = item.IsPicked,
                    CartDetails = dishesList



                }) ;
             
            }
            combinedModel.Order = disList;
            
            return View(combinedModel);

        }
        [HttpPost]
        public ActionResult AcceptOrder(int? id)
        {
            if (id == null)
            {
                return Json("Bad request", JsonRequestBehavior.AllowGet);
            }
            string result = delMngr.AcceptOrderRequest(id, Session["DeliveryBoy"].ToString());
            if (result == "success")
            {
                return Json("Request Accepted", JsonRequestBehavior.AllowGet);
            }
            else if(result== "Not free")
            {
                return Json("Sorry you can not accept is order as you have pending order to deliver", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed to accept request", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AcceptedOrders()
        {
            List<tbl_OrderDetails> retList = delMngr.GetAcceptedOrders(Session["DeliveryBoy"].ToString());
            List<OrderDetails> disList = new List<OrderDetails>();
            TempData["NewRequest"] = delMngr.GetRequestsCount(Session["DeliveryBoy"].ToString());
            dynamic combinedModel = new ExpandoObject();
            List<Cart> dishesList = new List<Cart>();
            List<PhoneNumbers> disPhList = new List<PhoneNumbers>();

            CustomerAddresses cusAddObj = new CustomerAddresses();
            List<PhoneNumbers> phoneList = new List<PhoneNumbers>();
            foreach (var item in retList)
            {
                disList.Add(new OrderDetails
                {
                    fk_OrderId = item.OrderId,
                    TotalAmount = item.TotalAmount,
                    CusName = item.tbl_Customer.CusName,
                    IsOrderConfirmed = item.IsOrderConfirmed,
                    Orderdate = item.Orderdate,
                    IsDelivered = item.IsDelivered,
                    IsPicked=item.IsPicked


                });
                foreach (var dish in item.tbl_OrderedFoodDetails)
                {
                    dishesList.Add(new Cart
                    {
                        DishName = dish.tbl_Dishes.DishName,
                        Quantity = dish.DishQuantity,
                        DishId = Convert.ToInt32(dish.fk_DishId)

                    });
                }
                tbl_Addresses obj = addMngr.GetAddressById(Convert.ToInt32(item.Order_fk_AddId));

                cusAddObj.DoorOrFlatNo = obj.DoorOrFlatNo;
                cusAddObj.LandMark = obj.LandMark;
                cusAddObj.PinCode = obj.PinCode;
                cusAddObj.AddressTypeName = obj.tbl_AddressType.TypeName;
                cusAddObj.Add_fk_CusId = obj.Add_fk_CusId;
                
                List<tbl_PhoneNumbers> retPhList = addMngr.GetPhoneNumberByCusId(obj.Add_fk_CusId);
                
                foreach(var ph in retPhList)
                {
                    disPhList.Add(new PhoneNumbers
                    {
                        PhoneNumber = ph.PhoneNumbers
                    });
                                                                
                }
                
                
            }
            combinedModel.Order = disList;
            combinedModel.Cart = dishesList;
            combinedModel.Phone = disPhList;
            combinedModel.Address = cusAddObj;
            return View(combinedModel);
        }




        public ActionResult DeliveredOrders()
        {
            List<tbl_OrderDetails> retList = delMngr.GetDeliveredOrders(Session["DeliveryBoy"].ToString());
            List<OrderDetails> disList = new List<OrderDetails>();
            TempData["NewRequest"] = delMngr.GetRequestsCount(Session["DeliveryBoy"].ToString());
            dynamic combinedModel = new ExpandoObject();
            List<Cart> dishesList = new List<Cart>();
            foreach (var item in retList)
            {
                disList.Add(new OrderDetails
                {
                    fk_OrderId = item.OrderId,
                    TotalAmount = item.TotalAmount,
                    CusName = item.tbl_Customer.CusName,
                    IsOrderConfirmed = item.IsOrderConfirmed,
                    Orderdate = item.Orderdate,
                    IsDelivered = item.IsDelivered,
                    RestName=item.tbl_Restaurant.RestName


                });
                foreach (var dish in item.tbl_OrderedFoodDetails)
                {
                    dishesList.Add(new Cart
                    {
                        DishName = dish.tbl_Dishes.DishName,
                        Quantity = dish.DishQuantity,
                        DishId = Convert.ToInt32(dish.fk_DishId)

                    });
                }
            }
            combinedModel.Order = disList;
            combinedModel.Cart = dishesList;
            return View(combinedModel);
        }


        public PartialViewResult _AddOtpFormView(int id)
        {
            OrderDetails regObj = new OrderDetails();
            regObj.id = id;
            return PartialView("_AddOtpFormView", regObj);
        }

        [HttpPost]
        public ActionResult _AddOtpFormView(OrderDetails obj)
        {
            if (obj == null)
            {
                return Json("please enter the otp", JsonRequestBehavior.AllowGet);
            }
            tbl_OrderDetails retObj = delMngr.GetOrderById(obj.id);
            if (retObj.OrderOtp == obj.OrderOtp)
            {
                int result = delMngr.ConfirmOrder(obj.id);
                if (result > 0)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }

                return Json("Error occured!!", JsonRequestBehavior.AllowGet);
            }
            


            return Json("Incorrect otp", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult OrderPicked(int id)
        {
            string result = delMngr.OrderPickStatusChange(id);
            if(result=="Not found")
            {
                return Json("Order not found", JsonRequestBehavior.AllowGet);
            }
            else if (result == "Success")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed to pick order", JsonRequestBehavior.AllowGet);
            }
        
        }




    }
}