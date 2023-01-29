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




    }
}