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
using System.Threading.Tasks;
using System.Dynamic;

namespace FoodDeliveryWebApplication.Controllers
{
    public class RestaurantController : Controller
    {
        RestaurantManager restMngr = new RestaurantManager();
        DishesManager dishMngr = new DishesManager();

        // Registration Section
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
            
            tbl_Restaurant insObj = new tbl_Restaurant();
            insObj.RestName = obj.Name;
            insObj.RestPhone = obj.RestPhone;
            insObj.RestEmail = obj.EmailId;
            insObj.RestPincode = obj.Pincode;
            string savePath = Server.MapPath("~/Content/RestaurantProfilePictures");
            string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
            obj.ImgUrl.SaveAs(saveThumbImagePath);
            insObj.RestImage = "~/Content/RestaurantProfilePictures/" + obj.ImgUrl.FileName;
            insObj.RestCountry = restObj.RestCountry;
            insObj.RestState = restObj.RestState;
            insObj.RestDistrict = restObj.RestDistrict;
            insObj.RestPassword = obj.Password;
            insObj.RestStatus = "A";
            insObj.RestTradeLicense = obj.RestTradeLicense;
            insObj.RestRole = 3;
            insObj.RestArea = obj.RestArea;
            insObj.IsValid = "No";
            insObj.Rest_fk_Offer = 5;
            var isExistEmail = restMngr.IsExistEmail(insObj.RestEmail.ToString());
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
                    return RedirectToAction("Login","Login");
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



        //Operations Section

        public ActionResult AddDish()
        {
            if (Session["Restaurant"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Home");

        }
        public ActionResult OrderHistory()
        {
            List<tbl_OrderDetails> retList = restMngr.GetOrderHistory(Session["Restaurant"].ToString());
            List<OrderDetails> disList = new List<OrderDetails>();
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
                    Orderdate = item.Orderdate


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
        public ActionResult PendingOrders()
        {
            List<tbl_OrderDetails> retList = restMngr.GetAllPendingOrders(Session["Restaurant"].ToString());
            List<OrderDetails> disList = new List<OrderDetails>();
            dynamic combinedModel = new ExpandoObject();
            List<Cart> dishesList = new List<Cart>();
            foreach(var item in retList)
            {
                disList.Add(new OrderDetails
                {
                    fk_OrderId = item.OrderId,
                    TotalAmount = item.TotalAmount,
                    CusName = item.tbl_Customer.CusName,
                    IsOrderConfirmed = item.IsOrderConfirmed,
                    Orderdate = item.Orderdate


                }) ;
                foreach(var dish in item.tbl_OrderedFoodDetails)
                {
                    dishesList.Add(new Cart
                    {
                        DishName = dish.tbl_Dishes.DishName,
                        Quantity = dish.DishQuantity,
                        DishId=Convert.ToInt32(dish.fk_DishId)
                        
                    });
                }
            }
            combinedModel.Order = disList;
            combinedModel.Cart = dishesList;
            return View(combinedModel);
            
        }

        [HttpPost]
        public ActionResult ConfirmOrder(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            string result = restMngr.ConfirmOrderbyRest(id);
            if (result == "Success")
            {
                return Json("Confirmed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }           

        }

        public ActionResult Restaurants()
        {
            if (Session["Customer"] != null)
            {
                List<tbl_Restaurant> nearestRestList = restMngr.GetNearestRestaurantDetails(Session["Customer"].ToString());
                List<Restaurant> disNearestRestList = new List<Restaurant>();
                foreach(var item in nearestRestList)
                {
                    disNearestRestList.Add(new Restaurant
                    {
                        Image = item.RestImage,
                        Name = item.RestName,
                        Id = item.RestId


                    });
                }
                return View(disNearestRestList);
            }
            List<tbl_Restaurant> returnList = restMngr.GetRestaurantDetails();
            if (returnList == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            List<Restaurant> displayList = new List<Restaurant>();
            foreach(var item in returnList)
            {
                displayList.Add(new Restaurant
                {
                    Image=item.RestImage,
                    Name=item.RestName,
                    Id=item.RestId

                });
            }


            return View(displayList);
        }
        //[HttpPost]

        //public ActionResult Restaurants(int id)
        //{
           

        //}


        


    }
}
