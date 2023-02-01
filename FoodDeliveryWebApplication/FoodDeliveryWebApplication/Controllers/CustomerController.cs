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
using System.Dynamic;



namespace FoodDeliveryWebApplication.Controllers
{
    public class CustomerController : Controller
    {
        CustomerManager cusMngr = new CustomerManager();
        EmailVerification emailVer = new EmailVerification();
        FavouriteRestaurantManager favRestMngr = new FavouriteRestaurantManager();
        CartManager cartMngr = new CartManager();
        AddressManager addMngr = new AddressManager();
        DishesManager dishMngr = new DishesManager();
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
               
                string savePath = Server.MapPath("~/Content/CustomerProfilePictures");
                string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
                obj.ImgUrl.SaveAs(saveThumbImagePath);
                insObj.CusImage = "~/Content/CustomerProfilePictures/" + obj.ImgUrl.FileName;
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

        public ActionResult FoodItems()
        {
            Session["CartItemsCount"] = cartMngr.GetCartItemsCount(Session["Customer"].ToString());
            return RedirectToAction("Restaurants", "Restaurant");
        }

        public ActionResult UserProfile()
        {
            dynamic combinedModel = new ExpandoObject();
            tbl_Customer retObj = cusMngr.GetCustomerDetails(Session["Customer"].ToString());
            User disObj = new User();
            disObj.Id = retObj.CusId;
            disObj.Name = retObj.CusName;
            disObj.Pincode = retObj.CusPincode;
            disObj.Image = retObj.CusImage;

            List<tbl_Addresses> retAddList = addMngr.GetAllAddresses(Session["Customer"].ToString());
            List<CustomerAddresses> disAddList = new List<CustomerAddresses>();
            foreach(var item in retAddList)
            {
                disAddList.Add(new CustomerAddresses
                {
                    AddId = item.AddId,
                    AddressTypeName=item.tbl_AddressType.TypeName,
                    
                    Add_fk_CusId = item.Add_fk_CusId,
                    LandMark = item.LandMark,
                    DoorOrFlatNo=item.DoorOrFlatNo,
                    PinCode=item.PinCode
                }) ;
            }


            List<tbl_PhoneNumbers> retPhList = cusMngr.GetAllPhoneNos(Session["Customer"].ToString());

            List<PhoneNumbers> disPhList = new List<PhoneNumbers>();
            foreach(var item in retPhList)
            {
                disPhList.Add(new PhoneNumbers
                {
                    PhoneId=item.PhoneId,
                    PhoneNumber=item.PhoneNumbers,
                    Phn_fk_CusId=item.Phn_fk_CusId

                });
            }

            combinedModel.Customer = disObj;
            combinedModel.Phone = disPhList;
            combinedModel.Address = disAddList;
            return View(combinedModel);
        }
        public ActionResult FavRestaurants()
        {
            List<tbl_Restaurant> _list = favRestMngr.GetFavListByCusId(Session["Customer"].ToString());
            List<Restaurant> displayList = new List<Restaurant>();
            int restCount = 0;
            foreach(var item in _list)
            {
                displayList.Add(new Restaurant
                {
                    Name = item.RestName,
                    RestArea = item.RestArea,
                    RestState=item.RestState,
                    RestDistrict=item.RestDistrict,
                    Image=item.RestImage,
                    Id=item.RestId
                    
                });
                restCount++;
             
            }
            ViewBag.count = restCount.ToString();

            return View(displayList);
        }
        public ActionResult ManageAddress(int id)
        {
            return RedirectToAction("InsertAddress", "Address", new { id = id });
        }

        
        public ActionResult InsertNumber()
        {
            return RedirectToAction("AddPhoneNo", "Address");
        }

        //public ActionResult DeleteNumber(int? id)
        //{
        //    return RedirectToAction("DeletePhoneNo", "Address",new {phId=id });
        //}



        public ActionResult EditProfile(int? id)
        {
            tbl_Customer retObj = cusMngr.GetCustomerById(Convert.ToInt32(id));
            if (retObj == null)
            {
                return HttpNotFound();
            }
            UserEdit disObj = new UserEdit();
            disObj.Name = retObj.CusName;
            disObj.Pincode = retObj.CusPincode;
            
            return View(disObj);
     
        }
        [HttpPost]
        public ActionResult EditProfile(UserEdit obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      
            if (ModelState.IsValid)
            {
                tbl_Customer updObj = new tbl_Customer();
                updObj.CusName = obj.Name;
                string savePath = Server.MapPath("~/Content/CustomerProfilePictures");
                string saveThumbImagePath = savePath + @"/" + obj.ImgUrl.FileName;
                obj.ImgUrl.SaveAs(saveThumbImagePath);
                updObj.CusImage = "~/Content/CustomerProfilePictures/" + obj.ImgUrl.FileName;
                updObj.CusPincode = obj.Pincode;
                updObj.CusEmail = Session["Customer"].ToString();
                string result = cusMngr.UpdateProfile(updObj);
                if (result == "Success")
                {
                    return RedirectToAction("UserProfile", "Customer");
                }
                else
                {
                    ViewBag.msg = "Failed to insert";
                    return View();
                }

            }
            return View();
        }

        public ActionResult CartItems()
        {
            return RedirectToAction("CartItemsDisplay", "Cart");
        }

        public ActionResult OrdersHistory()
        {
            dynamic combinedModel = new ExpandoObject();
            List<tbl_OrderDetails> retList = cusMngr.GetAllOrdersHistoryByUserEmail(Session["Customer"].ToString());
            List<OrderDetails> orderList = new List<OrderDetails>();
            List<Dishes> dishList = new List<Dishes>();
            foreach(var item in retList)
            {
                orderList.Add(new OrderDetails
                {
                    id = item.OrderId,
                    RestName = item.tbl_Restaurant.RestName,
                    Image=item.tbl_Restaurant.RestImage,
                    Order_fk_RestId=item.Order_fk_RestId,
                    TotalAmount = item.TotalAmount,
                    Orderdate=item.Orderdate,
                    PaymentMode=item.PaymentMode,
                    IsDelivered=item.IsDelivered


                }) ;

                foreach(var dish in item.tbl_OrderedFoodDetails)
                {
                    tbl_Dishes obj = dishMngr.GetDishById(Convert.ToInt32(dish.fk_DishId));
                    dishList.Add(new Dishes
                    {
                        DishImage=obj.DishImage,
                        DishDesc=obj.DishDesc,
                        DishName=obj.DishName,
                        DishPrice=obj.DishPrice

                    });
                }
            }
            combinedModel.Dishes = dishList;
            combinedModel.Orders = orderList;
            return View(combinedModel);
        }

        public ActionResult ActiveOrders()
        {
            dynamic combinedModel = new ExpandoObject();
            List<tbl_OrderDetails> retList = cusMngr.GetAllActiveOrdersByUserEmail(Session["Customer"].ToString());
            List<OrderDetails> orderList = new List<OrderDetails>();
            List<Dishes> dishList = new List<Dishes>();
            foreach (var item in retList)
            {
                orderList.Add(new OrderDetails
                {
                    id = item.OrderId,
                    RestName = item.tbl_Restaurant.RestName,
                    Image = item.tbl_Restaurant.RestImage,
                    Order_fk_RestId = item.Order_fk_RestId,
                    TotalAmount = item.TotalAmount,
                    Orderdate = item.Orderdate,
                    PaymentMode = item.PaymentMode,
                    IsDelivered = item.IsDelivered,
                    OrderOtp = item.OrderOtp


                }) ;

                foreach (var dish in item.tbl_OrderedFoodDetails)
                {
                    tbl_Dishes obj = dishMngr.GetDishById(Convert.ToInt32(dish.fk_DishId));
                    dishList.Add(new Dishes
                    {
                        DishImage = obj.DishImage,
                        DishDesc = obj.DishDesc,
                        DishName = obj.DishName,
                        DishPrice = obj.DishPrice

                    });
                }
            }
            combinedModel.Dishes = dishList;
            combinedModel.Orders = orderList;
            return View(combinedModel);
        }









    }
}