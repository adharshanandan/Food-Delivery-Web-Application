using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL.Manager;
using DAL.Models;
using FoodDeliveryWebApplication.Models;

namespace FoodDeliveryWebApplication.Controllers
{
    public class AdminController : Controller
    {
        CategoryManager catMngr = new CategoryManager();
        RestaurantManager restMngr = new RestaurantManager();
        DeliveryBoyManager delMngr = new DeliveryBoyManager();
        HomeManager homeMngr = new HomeManager();
        // GET: Admin
        public ActionResult CategoryList()
        {
            if (Session["Admin"] != null)
            {
                List<tbl_Category> returnList = catMngr.GetCategoryList();
                List<Category> displayList = new List<Category>();
                foreach (var item in returnList)
                {
                    displayList.Add(new Category
                    {
                        CatId = item.CatId,
                        CatName = item.CatName,
                        CatImage = item.CatImage,
                        CatStatus = item.CatStatus

                    });
                }

                return View(displayList);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public  ActionResult AddCategory(int id = 0)
        {
            if (Session["Admin"] != null)
            {
                if (id == 0)
                {

                    return View(new Category { CatId = 0 });

                }
                else
                {
                    tbl_Category obj = catMngr.GetCatById(id);                  
                    Category displayObj = new Category();
                    displayObj.CatId = obj.CatId;
                    displayObj.CatName = obj.CatName;
                    displayObj.CatImage = obj.CatImage;
                    return View(displayObj);


                }

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            

        }
        [HttpPost]
        public ActionResult AddCategory(Category obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            tbl_Category insObj = new tbl_Category();
            if (ModelState.IsValid)
            {
                string result;
                insObj.CatName = obj.CatName;
                string savePath = Server.MapPath("~/Content/CategoryImages");
                string saveThumbImagePath = savePath + @"/" + obj.CatImgUrl.FileName;
                obj.CatImgUrl.SaveAs(saveThumbImagePath);
                insObj.CatImage = "~/Content/CategoryImages/" + obj.CatImgUrl.FileName;
                insObj.CatStatus = "A";
                if (obj.CatId > 0)
                {
                    result = catMngr.UpdateCategory(insObj);
                    if (result == "Success")
                    {
                        ViewBag.Success = "Updated Successfully";
                        return View();
                        
                    }
                    else
                    {
                        ViewBag.Failed = "failed to update category!!";
                        return View();
                    }
                }
                else
                {
                    result = catMngr.InsertCategory(insObj);
                    if (result == "Success")
                    {
                        ViewBag.Success = "Insertef Successfully";
                        return View();
                    }
                    else
                    {
                        ViewBag.Failed = "failed to insert category!!";
                        return View();
                    }

                }

            }
            else
            {
                ViewBag.Status = "Please fill all fields";
                return View();

            }



        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Category obj = catMngr.GetCatById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string result = catMngr.DeleteCatById(id);
            if (result == "Success")
            {
                return RedirectToAction("CategoryList");
            }
            else
            {
                ViewBag.DeleteStatus = "Can not be deleted at this moment";
                return View();

            }


        }
        public List<Restaurant> GetRestAppList()
        {
            List<tbl_Restaurant> retList = restMngr.GetNotApprovedRest();
            List<Restaurant> disList = new List<Restaurant>();

            foreach (var item in retList)
            {
                disList.Add(new Restaurant
                {
                    Id = item.RestId,
                    Name = item.RestName,
                    EmailId = item.RestEmail,
                    RestPhone = item.RestPhone,
                    RestTradeLicense = item.RestTradeLicense,
                    RestArea = item.RestArea,
                    RestState = item.RestState


                });
            }
            return disList;
        }
        public ActionResult ApprovalPendingRest(int? id)
        {
          
            
            if (id == null)
            {
                return View(GetRestAppList());
            }
            
            else
            {
                int result = restMngr.ApproveRestaurant(id);
                if (result > 0)
                {
                    ViewBag.msg = "Approved";
                    return View(GetRestAppList());
                }
                else
                {
                    ViewBag.msg = "Approval failed";
                    return View(GetRestAppList());
                }
            }
            
        }


        public List<DeliveryGuy> GetDelBoyAppList()
        {
            List<tbl_DeliveryStaffs> retList = delMngr.GetDelBoysListToApprove();
            List<DeliveryGuy> disList = new List<DeliveryGuy>();
            foreach (var item in retList)
            {
                disList.Add(new DeliveryGuy
                {
                    Id = item.StaffId,
                    Name = item.StaffName,
                    EmailId = item.StaffEmail,
                    PhoneNo = item.StaffPhone,
                    DrivingLicense = item.DrivingLicense,
                    AdhaarNo = item.AdhaarNo,
                    StaffArea = item.StaffArea,
                    StaffState = item.StaffState

                });
            }
            return disList;
        }


 
        public ActionResult ApprovalPendingDelBoys(int? id)
        {
            if (id == null)
            {
                return View(GetDelBoyAppList());
            }
            int result = delMngr.ApproveDelboys(id);
            if (result > 0)
            {
                ViewBag.msg = "Approved";
                return View(GetDelBoyAppList());
            }
            else
            {
                ViewBag.msg = "Approval failed";
                return View(GetDelBoyAppList());
            }
        }


        public ActionResult DisplayContactUsList()
        {
            List<tbl_ContactUs> retList = homeMngr.GetAllContactUsList();
            List<ContactUs> disList = new List<ContactUs>();
            foreach(var item in retList)
            {
                disList.Add(new ContactUs
                {
                    Name=item.ContName,
                    EmailId=item.ContEmail,
                    Message=item.ContMsg
                });
            }
            return View(disList);
        }





    }
}