using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Manager;
using FoodDeliveryWebApplication.Models;
using System.Dynamic;

namespace FoodDeliveryWebApplication.Controllers
{
    public class CartController : Controller
    {
        CustomerManager cusMngr = new CustomerManager();
        CartManager cartMngr = new CartManager();
        AddressManager addMngr = new AddressManager();
        RestaurantManager restMngr = new RestaurantManager();
        // GET: Cart
        [HttpPost]
        public ActionResult AddtoCart(int? RestId,int? dishId)
        {
           
           
            if (Session["Customer"] != null)
            {
                if (dishId == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                List<tbl_Cart> checkList = cartMngr.GetCartList(Session["Customer"].ToString());
                if (checkList.Count!=0)
                {
                    if (!checkList.Where(e => e.Cart_fk_RestId == RestId).Any())
                    {
                        return Json("You have one restaurant dishes in your cart. Please clear your cart and try again", JsonRequestBehavior.AllowGet);
                    }

                }
                
                tbl_Cart insObj = new tbl_Cart();
                insObj.AddedDate = DateTime.Now;
                insObj.Cart_fk_CusId = Convert.ToInt32(cusMngr.GetCustomerIdByEmailId(Session["Customer"].ToString()));
                insObj.Cart_fk_DishId = dishId;
                insObj.Cart_fk_RestId = RestId;
                insObj.Quantity = 1;
                string result = cartMngr.AddItemCart(insObj);
                if (result == "Success")
                {
                    Session["CartItemsCount"] = cartMngr.GetCartItemsCount(Session["Customer"].ToString());
                    return Json("Added to cart.", JsonRequestBehavior.AllowGet);
                }
                else if (result == "Exists")
                {
                    return Json("Already exists in your cart", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Can not be added to cart at this moment", JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                return Json("login", JsonRequestBehavior.AllowGet);
                
            }
            
        }
        public ActionResult CartItemsDisplay()
        {
            List<tbl_Cart> retList = cartMngr.GetUserCartList(Session["Customer"].ToString());
            List<Cart> disList = new List<Cart>();
           
            dynamic combinedModel = new ExpandoObject();
            foreach (var item in retList)
            {
                disList.Add(new Cart
                {
                    DishImage = item.tbl_Dishes.DishImage,
                    AddedDate = item.AddedDate,
                    Quantity = item.Quantity,
                    RestaurantName = item.tbl_Restaurant.RestName,
                    DishPrice = item.tbl_Dishes.DishPrice,
                    CartId = item.CartId,
                    DishDesc = item.tbl_Dishes.DishDesc,
                    VegorNonveg = item.tbl_Dishes.VegOrNonveg,
                    DishName = item.tbl_Dishes.DishName


                }) ;
            }
            if (disList.Count!=0)
            {
                List<tbl_Addresses> retAddList = addMngr.GetAllAddresses(Session["Customer"].ToString());
                List<CustomerAddresses> disAddList = new List<CustomerAddresses>();
                PayAmount payObj = new PayAmount();

                foreach(var item in retAddList)
                {
                    disAddList.Add(new CustomerAddresses
                    {
                        AddId=item.AddId,
                        DoorOrFlatNo=item.DoorOrFlatNo,
                        LandMark=item.LandMark,
                        TypeId=item.AddressType,
                        TypeName=item.tbl_AddressType.TypeName,
                        PinCode=item.PinCode
                    });
                }
                payObj.TotalAmount = 0;
                foreach (var item in disList)
                {
                    payObj.TotalAmount = payObj.TotalAmount + (item.DishPrice * Convert.ToDecimal(item.Quantity));

                }
                              
                tbl_Cart retObj = cartMngr.GetCartRestDetails(Session["Customer"].ToString());
                tbl_Restaurant offerObj = restMngr.RestaurantOfferDetails(retObj.Cart_fk_RestId);
                payObj.offerPercentage = 0;
                if (offerObj != null)
                {
                    decimal offerPercentage = Convert.ToDecimal(offerObj.tbl_Offers.OfferPercentage) / Convert.ToDecimal(100);
                    payObj.TotalAmount = payObj.TotalAmount - (payObj.TotalAmount * Convert.ToDecimal(offerPercentage));
                    payObj.offerPercentage = payObj.TotalAmount * Convert.ToDecimal(offerPercentage);
                }
                payObj.ToPay = payObj.TotalAmount + payObj.Tax + payObj.DelveryCharge-payObj.offerPercentage;
                
                Cart disObj = new Cart();
                disObj.RestaurantName = retObj.tbl_Restaurant.RestName;
                disObj.Image = retObj.tbl_Restaurant.RestImage;

                combinedModel.CartItem = disList;
                combinedModel.RestDetails = disObj;
                combinedModel.PayAmount = payObj;
                combinedModel.Addresses = disAddList;


                return View(combinedModel);

            }
            return View();
         
        }
        [HttpPost]
        public ActionResult QuantityMinus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            string result = cartMngr.MinusQuantity(id);
            if (result == "Success")
            {
                return Json("Updated", JsonRequestBehavior.AllowGet);
            }
            else if (result=="Zero")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (result == "Not found")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult QuantityPlus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            string result = cartMngr.PlusQuantity(id);
            if (result == "Success")
            {
                
                return Json("Updated", JsonRequestBehavior.AllowGet);
            }
            else if (result == "Quantity over")
            {
                return Json("Over", JsonRequestBehavior.AllowGet);
            }
            else if (result == "Not found")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult RemoveCartItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            string result = cartMngr.RemoveItem(id);
            if (result == "Success")
            {
                Session["CartItemsCount"] = cartMngr.GetCartItemsCount(Session["Customer"].ToString());
                return Json("Deleted", JsonRequestBehavior.AllowGet);
            }
            else if (result == "Not found")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            string result = cartMngr.ClearCartItems(Session["Customer"].ToString());
            if (result == "Success")
            {
                Session["CartItemsCount"] = cartMngr.GetCartItemsCount(Session["Customer"].ToString());
                return Json("Cleared", JsonRequestBehavior.AllowGet);
            }
            else if (result == "Empty")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult PlaceOrder(OrderDetails obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            tbl_OrderDetails insObj = new tbl_OrderDetails();
            List<tbl_Cart> retList = cartMngr.GetCartList(Session["Customer"].ToString());
            string pincode = addMngr.PicodeCheckForDelivery(obj.Order_fk_AddId);            
            tbl_Cart retObj = cartMngr.GetCartRestDetails(Session["Customer"].ToString());
            if (pincode != retObj.tbl_Restaurant.RestPincode)
            {
                return Json("Delivery is not possible to this loation. Please choose a location matching the pincode of the restaurant", JsonRequestBehavior.AllowGet);

            }
            if (ModelState.IsValid)
            {
                
                insObj.Order_fk_RestId = retObj.Cart_fk_RestId;
                insObj.Order_fk_CusId = retObj.Cart_fk_CusId;
                insObj.Orderdate = DateTime.Now;
                insObj.IsDelivered = "N";
                insObj.IsOrderConfirmed = "N";
                insObj.IsPaid = "N";
                insObj.TotalAmount = obj.TotalAmount;
                Random RandNo = new Random();
                insObj.OrderOtp = RandNo.Next(100001, 999999).ToString();

                insObj.PaymentMode = obj.PaymentMode;
                foreach(var item in retList)
                {
                    insObj.tbl_OrderedFoodDetails.Add(new tbl_OrderedFoodDetails
                    {
                        fk_DishId=item.Cart_fk_DishId,
                        DishQuantity=item.Quantity,
                        fk_OrderId=insObj.OrderId
                    });
                }
                if (obj.PaymentMode == "Card")
                {
                    Session["OrderItem"] = insObj;
                    return RedirectToAction("SelectBankToPay", "Payment", new { insObj.TotalAmount });
                }
                string result = cartMngr.InsertOrderDetails(insObj);
                if (result == "Success")
                {                   
                    string clrResult=cartMngr.ClearCartItems(Session["Customer"].ToString());
                    if (clrResult == "Success")
                    {
                        return Json("Sent", JsonRequestBehavior.AllowGet);
                    }
                    

                }
                
            }
            return Json("please check if the address and payment method are selected or not",JsonRequestBehavior.AllowGet);
        }
    }
}