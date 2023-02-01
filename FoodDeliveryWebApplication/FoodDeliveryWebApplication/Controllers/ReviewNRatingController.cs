using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using FoodDeliveryWebApplication.Models;
using DAL.Manager;

namespace FoodDeliveryWebApplication.Controllers
{
    public class ReviewNRatingController : Controller
    {
        RestaurantManager restMngr = new RestaurantManager();
        // GET: ReviewNRating
        public ActionResult PostAReview(int id)
        {
            ReviewRating disObj = new ReviewRating();
            tbl_Restaurant retObj = restMngr.RestaurantDetailsById(id);
            if (retObj != null)
            {
                disObj.Rev_fk_RestId = retObj.RestId;
                disObj.RestName = retObj.RestName;
                disObj.RestImage = retObj.RestImage;
            }
            
            return View(disObj);
        }
    }
}