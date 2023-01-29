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
                if (checkList != null)
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
                    DishPrice = item.tbl_Dishes.DishPrice.ToString(),
                    DishDesc=item.tbl_Dishes.DishDesc,
                    VegorNonveg=item.tbl_Dishes.VegOrNonveg,
                    DishName=item.tbl_Dishes.DishName


                }) ;
            }

            tbl_Cart retObj = cartMngr.GetCartRestDetails(Session["Customer"].ToString());
            Cart disObj = new Cart();
            disObj.RestaurantName = retObj.tbl_Restaurant.RestName;
            disObj.Image = retObj.tbl_Restaurant.RestImage;

            combinedModel.CartItem = disList;
            combinedModel.RestDetails = disObj;

            return View(combinedModel);
        }
    }
}