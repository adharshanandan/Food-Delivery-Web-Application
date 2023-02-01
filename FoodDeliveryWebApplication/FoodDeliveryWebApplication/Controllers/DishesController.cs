using DAL.Manager;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodDeliveryWebApplication.Models;
using System.Threading.Tasks;
using System.Dynamic;
using System.Net;



namespace FoodDeliveryWebApplication.Controllers
{
    public class DishesController : Controller
    {

        DishesManager dishMngr = new DishesManager();
        RestaurantManager restMngr = new RestaurantManager();
        CartManager cartMngr = new CartManager();

        // To display in restaurant accounts
        public ActionResult DishList(string search)
        {
            

            if (Session["Restaurant"] != null)
            {
                List<Dishes> displayList = new List<Dishes>();
                List<tbl_Dishes> returnList= dishMngr.GetDishesDetails(search, Session["Restaurant"].ToString());
                foreach(var item in returnList)
                {
                    displayList.Add(new Dishes
                    {
                        DishId=item.DishId,
                        DishName=item.DishName,
                        DishDesc=item.DishDesc,
                        DishImage=item.DishImage,
                      
                        DishCategory=item.tbl_Category.CatName,
                        VegorNonveg=item.VegOrNonveg,
                        DishPrice=item.DishPrice

                    });
                }
               
                return View(displayList);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // Method to add a dish
        public ActionResult Create(int id=0)
        {
            if (Session["Restaurant"] != null)
            {
                if (id == 0)
                {
                    
                    BindCategory();
                   
                    BindVegorNonVeg();
                    return View(new Dishes { DishId=0});

                }
                else
                {
                    tbl_Dishes obj = dishMngr.GetDishById(id);
                    BindCategory(obj.Dish_fk_Cat);
                    
                    BindVegorNonVeg(obj.VegOrNonveg);
                    Dishes displayObj = new Dishes();
                    displayObj.DishName = obj.DishName;
                    displayObj.DishDesc = obj.DishDesc;
                    displayObj.DishId = obj.DishId;
                    displayObj.DishPrice = obj.DishPrice;
                   
                    return View(displayObj);
                    

                }
                
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
                
        }

        [HttpPost]
        public ActionResult Create(Dishes obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            tbl_Dishes insObj = new tbl_Dishes();
            insObj.DishId = obj.DishId;
            if (ModelState.IsValid)
            {
                string result;
                insObj.DishName = obj.DishName;
                insObj.DishDesc = obj.DishDesc;
                            
                string savePath = Server.MapPath("~/Content/DishesImages");
                string saveThumbImagePath = savePath + @"/" + obj.DishImgUrl.FileName;
                obj.DishImgUrl.SaveAs(saveThumbImagePath);
                insObj.DishImage = "~/Content/DishesImages/"+ obj.DishImgUrl.FileName;
                insObj.DishPrice = Convert.ToDecimal(obj.DishPrice);
                insObj.VegOrNonveg = obj.VegorNonveg;
               
                insObj.Dish_fk_Cat = obj.DishCategoryId;
                if (insObj.DishId > 0)
                {
                    result = dishMngr.DishUpdate(insObj, Session["Restaurant"].ToString());
                    if (result == "Updated")
                    {
                        TempData["CusSuccess"] = "Updated successfully";
                        return RedirectToAction("Create");
                    }
                    else if (result == "Failed")
                    {
                        return RedirectToAction("Create");
                    }          
                    else
                    {
                        return RedirectToAction("Create");
                    }

                }
                else
                {
                    result = dishMngr.DishInsert(insObj, Session["Restaurant"].ToString());
                    if (result == "Success")
                    {
                        TempData["CusSuccess"] = "Registered successfully. Please check your Email to activate your account";
                        return RedirectToAction("Create");
                    }
                    else if (result == "Failed")
                    {
                        return RedirectToAction("Create");
                    }
                    else if (result == "Exist")
                    {
                        ViewBag.exist = "Email already exists. Please login !";
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        return RedirectToAction("Create");
                    }

                }
                

            }
            return RedirectToAction("Create");
        }


        //Method to delete a dish
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Dishes obj = dishMngr.GetDishById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string result = dishMngr.DeleteDishById(id);
            if (result == "Deleted")
            {
                return RedirectToAction("DishList");
            }
            else
            {
                return View();
            }


        }
        
        

        //Method to bind categories to drop down list

        public void BindCategory(int id = 0)
        {
            List<SelectListItem> ddl_CategoryList = new List<SelectListItem>();
            List<tbl_Category> _returnList = dishMngr.GetCategoryList();
            if (id > 0)
            {
                tbl_Category returnObj = dishMngr.GetCategoryById(id);
                foreach (var obj in _returnList)
                {
                    ddl_CategoryList.Add(new SelectListItem
                    {
                        Selected = (obj.CatId == id ? true : false),
                        Text = obj.CatName,
                        Value = obj.CatId.ToString()

                    });
                }


            }
            else
            {
                ddl_CategoryList.Add(new SelectListItem
                {
                    Selected = true,
                    Text = "-- Select Category --",
                    Value = ""
                });
                foreach (var obj in _returnList)
                {
                    ddl_CategoryList.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = obj.CatName,
                        Value = obj.CatId.ToString()

                    });
                }
            }
            ViewBag.ddlCategory = ddl_CategoryList;

        }


        public void BindVegorNonVeg(string text="")
        {
            List<SelectListItem> _listType = new List<SelectListItem>();
            if (text != "")
            {
            
                _listType.Add(new SelectListItem
                {
                    Selected = ("Veg" == text ? true : false),
                    Text = "Veg",
                    Value = "Veg"

                });
                _listType.Add(new SelectListItem
                {
                    Selected = ("Non-Veg" == text ? true : false),
                    Text = "Non-Veg",
                    Value = "Non-Veg"

                });

            }
            else
            {
                _listType.Add(new SelectListItem
                {
                    Selected = true,
                    Text = "-- Select Veg or Non-Veg --",
                    Value = ""

                });

                _listType.Add(new SelectListItem
                {
                    Selected =false,
                    Text = "Veg",
                    Value = "Veg"

                });
                _listType.Add(new SelectListItem
                {
                    Selected = false,
                    Text = "Non-Veg",
                    Value = "Non-Veg"

                });


            }
            ViewBag.rbVegorNon = _listType;
        }


        //Method to display food items to buy
        public ActionResult FoodItems(int id, string search = "",string vegOrNonVeg="",int? catId=0)
        {
            
            List<tbl_Dishes> returnList = dishMngr.DishesForDisplay(search, id, vegOrNonVeg, catId);
            List<Dishes> displayList = new List<Dishes>();
            tbl_Restaurant returnRestObj = restMngr.RestaurantDetailsById(id);
            Restaurant displayRestObj = new Restaurant();
            displayRestObj.Name = returnRestObj.RestName;
            displayRestObj.RestArea = returnRestObj.RestArea;
            displayRestObj.RestDistrict = returnRestObj.RestDistrict;
            displayRestObj.RestState = returnRestObj.RestState;
            displayRestObj.Image = returnRestObj.RestImage;
            displayRestObj.Id = returnRestObj.RestId;
            displayRestObj.Offer = returnRestObj.tbl_Offers.OfferDescription;

            BindVegorNonVeg();
            BindCategory();

            //created a dynamic model to return restaurant and dishes model to view
            dynamic combinedModel = new ExpandoObject();
            
            if (Session["Customer"] != null)
            {
                displayRestObj.isFavourite = returnRestObj.tbl_FavRestaurants.Where(e => e.tbl_Customer.CusEmail == Session["Customer"].ToString()).Any() ? true : false;
                List<tbl_Cart> cartList = cartMngr.GetCartList(Session["Customer"].ToString());
                foreach(var item in returnList)
                {
                    //To check whether the displaying item is already in cart of current user or not
                    //By this name of the button is decided (Added or Add to Cart)
                    bool isExist = cartList.Where(e => e.Cart_fk_DishId == item.DishId).Any() ? true : false;

                    if (isExist)
                    {
                        displayList.Add(new Dishes
                        {
                            DishName = item.DishName,
                            DishDesc = item.DishDesc,
                            DishImage = item.DishImage,
                            DishPrice = item.DishPrice,


                            DishId = item.DishId,
                            buttonName = "Added"



                        }) ;

                    }
                    else
                    {
                        displayList.Add(new Dishes
                        {
                            DishName = item.DishName,
                            DishDesc = item.DishDesc,
                            DishImage = item.DishImage,
                            DishPrice = item.DishPrice,
                            
                            DishId = item.DishId,
                            buttonName = "Add"


                        });
                    }

                    
                   
                }
                TempData["RestId"] = id;
                combinedModel.Restaurant = displayRestObj;
                combinedModel.Dishes = displayList;
                return View(combinedModel);
            }
            
            //to view dishes as a guest user
            foreach (var item in returnList)
            {
                displayList.Add(new Dishes
                {
                    DishName = item.DishName,
                    DishDesc = item.DishDesc,
                    DishImage = item.DishImage,
                    DishPrice = item.DishPrice,
                   
                    DishId=item.DishId,
                    buttonName = "Add"


                }) ;

            }
            TempData["RestId"] = id;
            combinedModel.Restaurant = displayRestObj;
            combinedModel.Dishes = displayList;
            return View(combinedModel);
        }
        
    }
}