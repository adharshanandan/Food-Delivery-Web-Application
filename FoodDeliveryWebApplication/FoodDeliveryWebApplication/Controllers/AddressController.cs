using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Manager;
using DAL.Models;
using FoodDeliveryWebApplication.Models;
using System.Net;
using System.Threading.Tasks;

namespace FoodDeliveryWebApplication.Controllers
{
    public class AddressController : Controller
    {
        AddressManager addMngr = new AddressManager();
        public ActionResult InsertAddress(int id)
        {
            if (id == 0)
            {
                BindAddressType();
                return View(new CustomerAddresses { AddId = 0 });
            }
            else
            {
                tbl_Addresses retObj = addMngr.GetAddressById(Convert.ToInt32(id));
                BindAddressType(Convert.ToInt32(retObj.AddressType));
                CustomerAddresses disObj = new CustomerAddresses();
                disObj.AddId = retObj.AddId;
                disObj.AddressType = retObj.AddressType;
                disObj.PinCode = retObj.PinCode;
                disObj.LandMark = retObj.LandMark;
                disObj.DoorOrFlatNo = retObj.DoorOrFlatNo;
                disObj.Add_fk_CusId = retObj.Add_fk_CusId;
                if (retObj == null)
                {
                    return HttpNotFound();
                }
                return View(disObj);
            }

        }
        [HttpPost]
        public ActionResult AddAddress(CustomerAddresses obj)
        {
            if (obj == null)
            {
                return Json("invalid request", JsonRequestBehavior.AllowGet);
            }
            if (obj.AddId > 0)
            {
                if (ModelState.IsValid)
                {
                    tbl_Addresses editObj = new tbl_Addresses();
                    editObj.DoorOrFlatNo = obj.DoorOrFlatNo;
                    editObj.AddId = obj.AddId; ;
                    editObj.AddressType = obj.AddressType;
                    editObj.PinCode = obj.PinCode;
                    editObj.LandMark = obj.LandMark;
                    string result = addMngr.InsertAddress(editObj);
                    if (result == "Success")
                    {
                        
                        return Json("Successfully updated", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed to update", JsonRequestBehavior.AllowGet);
                    }

                }
                BindAddressType();
                return Json("Form validation failed", JsonRequestBehavior.AllowGet);


            }
            else
            {
                if (ModelState.IsValid)
                {
                    tbl_Addresses insObj = new tbl_Addresses();
                    insObj.AddId = 0;
                    insObj.DoorOrFlatNo = obj.DoorOrFlatNo;
                    insObj.AddressType = obj.AddressType;
                    insObj.PinCode = obj.PinCode;
                    insObj.LandMark = obj.LandMark;
                    string result = addMngr.InsertAddress(insObj, Session["Customer"].ToString());
                    if (result == "Success")
                    {

                        return Json("Successfully inserted",JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        
                        return Json("Failed to insert",JsonRequestBehavior.AllowGet);
                    }

                }
                return Json("Form validation failed", JsonRequestBehavior.AllowGet);

            }
           
        }
        public void BindAddressType(int id=0)
        {
            List<SelectListItem> ddl_AddressType = new List<SelectListItem>();
            List<tbl_AddressType> _list = addMngr.GetAddressTypes();
            if (id > 0)
            {
                foreach(var obj in _list)
                {
                    ddl_AddressType.Add(new SelectListItem
                    {
                        Selected = (obj.TypeId == id ? true : false),
                        Text = obj.TypeName,
                        Value = obj.TypeId.ToString()
                    });
                }
            }
            else
            {
                ddl_AddressType.Add(new SelectListItem
                {
                    Selected = true,
                    Text = "-- Select--",
                    Value = ""
                });
                foreach(var item in _list)
                {
                    ddl_AddressType.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = item.TypeName,
                        Value = item.TypeId.ToString()
                    });
                }
            }
            TempData["ddlAddressTypeList"] = ddl_AddressType;
        }

    

        [HttpPost]
        public ActionResult DeleteAddresses(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string result = addMngr.DeleteAddress(id);
            if (result == "Success")
            {
                return Json("Successfully Deleted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed to delete!", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddPhoneNo()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddPhoneNo(PhoneNumbers obj)
        {
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                tbl_PhoneNumbers insObj = new tbl_PhoneNumbers();
                insObj.PhoneNumbers = obj.PhoneNumber;
                string result = addMngr.InsertPhone(insObj, Session["Customer"].ToString());
                if (result == "Success")
                {
                    return RedirectToAction("UserProfile", "Customer");
                }
                else if (result == "Exist")
                {
                    ViewBag.msg = "Already Exists";
                    return View();
                }
                else
                {
                    ViewBag.msg = "Failed to insert";
                    return View();
                }

            }
            
            return View();
            
        }

        [HttpPost]
        public ActionResult DeletePhoneNo(int? phId)
        {

            if (phId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string result = addMngr.DeletePhone(phId);
            if (result == "Success")
            {
                return Json("Successfully Deleted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed to delete!", JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult _AddView()
        {
          
            BindAddressType();
            CustomerAddresses regObj = new CustomerAddresses();
            return PartialView("_AddView", regObj);


        }
        [HttpPost]
        public ActionResult _AddAddressFromAddView(CustomerAddresses obj)
        {
            if (obj == null)
            {
                return Json("Invalid content", JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                tbl_Addresses insObj = new tbl_Addresses();
                insObj.AddId = 0;
                insObj.DoorOrFlatNo = obj.DoorOrFlatNo;
                insObj.AddressType = obj.AddressType;
                insObj.PinCode = obj.PinCode;
                insObj.LandMark = obj.LandMark;
                string result = addMngr.InsertAddress(insObj, Session["Customer"].ToString());
                if (result == "Success")
                {
                    return Json("Inserted successfully", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            return Json("Please enter all details", JsonRequestBehavior.AllowGet);
            //if (obj.AddId > 0)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        tbl_Addresses editObj = new tbl_Addresses();
            //        editObj.DoorOrFlatNo = obj.DoorOrFlatNo;
            //        editObj.AddId = obj.AddId; ;
            //        editObj.AddressType = obj.AddressType;
            //        editObj.PinCode = obj.PinCode;
            //        editObj.LandMark = obj.LandMark;
            //        string result = addMngr.InsertAddress(editObj);
            //        if (result == "Success")
            //        {

            //            return Json("Successfully updated", JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            return Json("Failed to update", JsonRequestBehavior.AllowGet);
            //        }

            //    }
            //    BindAddressType();
            //    return View();


            //}





        }

     }
}