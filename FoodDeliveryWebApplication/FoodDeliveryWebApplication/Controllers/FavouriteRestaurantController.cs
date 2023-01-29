using DAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace FoodDeliveryWebApplication.Controllers
{
    public class FavouriteRestaurantController : Controller
    {
        FavouriteRestaurantManager FavRestMngr = new FavouriteRestaurantManager();
        CustomerManager cusmngr = new CustomerManager();
        [HttpPost]
        public ActionResult UnFavourite(int restId)
        {
            if (Session["Customer"] != null)
            {
                string cusEmail = Session["Customer"].ToString();
                string result = FavRestMngr.DeleteFromFavList(restId, cusEmail);
                if (result == "Success")
                {
                    return Json("Removed from favourite list",JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error occured!", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Login", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Favourite(int restId)
        {
            if (Session["Customer"] != null)
            {
                string cusId = cusmngr.GetCustomerIdByEmailId(Session["Customer"].ToString());
                tbl_FavRestaurants insObj = new tbl_FavRestaurants();
                insObj.Fav_fk_RestId = restId;
                insObj.Fav_fk_CusId = Convert.ToInt32(cusId);                
                string result = FavRestMngr.AddToFavList(insObj);
                if (result == "Success")
                {
                    return Json("Added to favourite list", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error occured!", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Login", JsonRequestBehavior.AllowGet);
            }
        }
    }
}