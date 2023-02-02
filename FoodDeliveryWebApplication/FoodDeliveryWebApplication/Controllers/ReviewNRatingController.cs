using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using FoodDeliveryWebApplication.Models;
using DAL.Manager;
using System.Dynamic;

namespace FoodDeliveryWebApplication.Controllers
{
    public class ReviewNRatingController : Controller
    {
        RestaurantManager restMngr = new RestaurantManager();
        CustomerManager cusMngr = new CustomerManager();
        ReviewManager revMngr = new ReviewManager();
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

        [HttpPost]
        public ActionResult PostAReview(ReviewRating obj)
        {
            tbl_ReviewAndRating insObj = new tbl_ReviewAndRating();
            insObj.ReviewContent = obj.ReviewContent;
            insObj.Rev_fk_CusId = Convert.ToInt32(cusMngr.GetCustomerIdByEmailId(Session["Customer"].ToString()));
            insObj.Rating = obj.Rating;
            insObj.Rev_fk_RestId = obj.Rev_fk_RestId;
            insObj.PostedDate = DateTime.Now;
            int result = revMngr.PostReviews(insObj);
            if (result > 0)
            {
                return Json("Posted successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error occured", JsonRequestBehavior.AllowGet);
            }
         
        }

        public ActionResult DisplayReviews(int? restId)
        {
            if (restId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            List<tbl_ReviewAndRating> revRetList = revMngr.GetReviewListByRestId(Convert.ToInt32(restId));
            List<ReviewRating> revDisList = new List<ReviewRating>();
            tbl_Restaurant retRestObj = restMngr.RestaurantDetailsById(Convert.ToInt32(restId));
            Restaurant disRestObj = new Restaurant();
            disRestObj.Name = retRestObj.RestName;
            disRestObj.Image = retRestObj.RestImage;
            foreach (var rev in revRetList)
            {
                revDisList.Add(new ReviewRating
                {
                    ReviewContent = rev.ReviewContent,
                    Rating = rev.Rating,
                    PostedDate = Convert.ToDateTime(rev.PostedDate),
                    CusName = rev.tbl_Customer.CusName,
                    RestName=rev.tbl_Restaurant.RestName,
                    RestImage=rev.tbl_Restaurant.RestImage

                }) ;
            }
            dynamic combinedModel = new ExpandoObject();
            combinedModel.Review = revDisList;
            combinedModel.Restaurant = disRestObj;
            return View(combinedModel);
        }
    }
}