using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Manager;
using FoodDeliveryWebApplication.Models;

namespace FoodDeliveryWebApplication.Controllers
{

    public class PaymentController : Controller
    {
        PaymentManager payMngr = new PaymentManager();
        CustomerManager cusMngr = new CustomerManager();
        RestaurantManager restMngr = new RestaurantManager();
        public ActionResult SelectBankToPay()
        {
            BindBankAccounts(Session["Customer"].ToString());
            BankAccounts obj = new BankAccounts();
            if (TempData["ddlUserBankList"] != null)
            {
                return View(obj);
            }
            else
            {
                return RedirectToAction("AddBankDetails");
            }
           
            
        }

        public ActionResult SelectBankToRefund(string orderId)
        {
            BindBankAccounts(Session["Customer"].ToString());
            BankAccounts obj = new BankAccounts();
            
            TempData["CancelOrderId"] = orderId;
            return View(obj);
        }
        public void BindBankAccounts(string EmailId)
        {
            List<SelectListItem> ddl_UserBanks = new List<SelectListItem>();
            List<tbl_UserBankAcc> _list = payMngr.GetAllBankAccountsofUser(EmailId);
            if (_list.Count > 0)
            {
                List<BankAccounts> disList = new List<BankAccounts>();
                foreach (var item in _list)
                {
                    disList.Add(new BankAccounts
                    {
                        AccId = item.id,
                        AccNumber = item.AccNumber
                    });
                }
                foreach (var acc in disList)
                {
                    char[] tempArray = acc.AccNumber.ToCharArray();
                    for (int i = tempArray.Length - 5; i >= 0; i--)
                    {
                        tempArray[i] = '*';
                    }
                    acc.AccNumber = new string(tempArray);

                }

                ddl_UserBanks.Add(new SelectListItem
                {
                    Selected = true,
                    Text = "-- Select Bank Account --",
                    Value = ""
                });
                foreach (var item in disList)
                {
                    ddl_UserBanks.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = item.AccNumber,
                        Value = item.AccId.ToString()
                    });
                }
                TempData["ddlUserBankList"] = ddl_UserBanks;
            }
            

           
            else
            {
                TempData["ddlUserBankList"] = null;
            }

        }

        public ActionResult UserBankAccounts()
        {
            if (Session["Customer"] != null)
            {
                List<tbl_UserBankAcc> retList = payMngr.GetAllBankAccountsofUser(Session["Customer"].ToString());
                List<BankAccounts> disList = new List<BankAccounts>();
                foreach (var item in retList)
                {
                    disList.Add(new BankAccounts
                    {
                        AccNumber = item.AccNumber,
                        Branch = item.Branch,
                        BankName = item.tbl_BankNames.BankName,
                        IfscCode = item.IfscCode,
                        AccId = item.id


                    });
                }
                foreach(var acc in disList)
                {
                    char[] tempArray = acc.AccNumber.ToCharArray();
                    for(int i = tempArray.Length - 5; i >= 0; i--)
                    {
                        tempArray[i] = '*';
                    }
                    acc.AccNumber = new string(tempArray);
                   
                }

                return View(disList);

            }
            else if (Session["Restaurant"] != null)
            {
                List<tbl_ResBankAcc> retList = payMngr.GetAllBankAccountsofRest(Session["Restaurant"].ToString());
                List<BankAccounts> disList = new List<BankAccounts>();
                foreach (var item in retList)
                {
                    disList.Add(new BankAccounts
                    {
                        AccNumber = item.AccNumber,
                        Branch = item.Branch,
                        BankName = item.tbl_BankNames.BankName,
                        IfscCode = item.IfscCode,
                        AccId = item.id
                    });
                }
                foreach (var acc in disList)
                {
                    char[] tempArray = acc.AccNumber.ToCharArray();
                    for (int i = tempArray.Length - 5; i >= 0; i--)
                    {
                        tempArray[i] = '*';
                    }
                    acc.AccNumber = new string(tempArray);

                }

                return View(disList);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        public ActionResult AddBankDetails(int id = 0)
        {
            if (Session["Customer"] != null)
            {
                BankAccounts disObj = new BankAccounts();
                if (id > 0)
                {
                    tbl_UserBankAcc retBankObj = payMngr.GetBankAccountById(id);
                    disObj.AccNumber = retBankObj.AccNumber;
                    disObj.IfscCode = retBankObj.IfscCode;
                    disObj.rder_fk_CusId = retBankObj.rder_fk_CusId;
                    disObj.Branch = retBankObj.Branch;
                    disObj.AccId = retBankObj.id;
                    BindBankNames(retBankObj.tbl_BankNames.BankId);
                    return View(disObj);


                }
                else
                {

                    BindBankNames();
                    return View(disObj);
                }

            }
            else if (Session["Restaurant"] != null)
            {
                BankAccounts disObj = new BankAccounts();
                if (id > 0)
                {
                    tbl_ResBankAcc retBankObj = payMngr.GetRestBankAccountById(id);
                    disObj.AccNumber = retBankObj.AccNumber;
                    disObj.IfscCode = retBankObj.IfscCode;
                    disObj.rder_fk_CusId = retBankObj.bank_fk_RestId;
                    disObj.Branch = retBankObj.Branch;
                    disObj.AccId = retBankObj.id;
                    BindBankNames(retBankObj.tbl_BankNames.BankId);
                    return View(disObj);


                }
                else
                {

                    BindBankNames();
                    return View(disObj);
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");

            }

        }
        [HttpPost]
        public ActionResult AddBankDetails(BankAccounts obj)
        {
            if (obj.AccId > 0)
            {
                if (Session["Customer"] != null)
                {
                    tbl_UserBankAcc editObj = new tbl_UserBankAcc();
                    if (ModelState.IsValid)
                    {
                        tbl_Customer cusObj = cusMngr.GetCustomerDetailsByEmailId(Session["Customer"].ToString());
                        editObj.rder_fk_CusId = cusObj.CusId;

                        editObj.AccNumber = obj.AccNumber;
                        editObj.Branch = obj.Branch;
                        editObj.IfscCode = obj.IfscCode;
                        editObj.User_fk_BankName = obj.fk_BankName;
                        editObj.id = obj.AccId;

                        int status = payMngr.AddOrEditUserBankAccounts(editObj);
                        if (status > 0)
                        {
                            ViewBag.bankAccEditMsg = "Updated Successfully";
                            return RedirectToAction("UserBankAccounts", "Payment");
                        }
                        else
                        {
                            ViewBag.bankAccEditMsg = "Failed to add";
                            BindBankNames();
                            return View();
                        }
                    }
                    BindBankNames();
                    return View();

                }
                else if (Session["Restaurant"] != null)
                {
                    tbl_ResBankAcc restEditObj = new tbl_ResBankAcc();
                    if (ModelState.IsValid)
                    {
                        tbl_Restaurant restObj = restMngr.IsExistEmail(Session["Restaurant"].ToString());
                        restEditObj.bank_fk_RestId = restObj.RestId;
                        restEditObj.AccNumber = obj.AccNumber;
                        restEditObj.Branch = obj.Branch;
                        restEditObj.IfscCode = obj.IfscCode;
                        restEditObj.Rest_fk_BankName = obj.fk_BankName;
                        restEditObj.id = obj.AccId;

                        int status = payMngr.AddOrEditRestBankAccounts(restEditObj);
                        if (status > 0)
                        {
                            ViewBag.bankAccRestEditMsg = "Updated Successfully";
                            return RedirectToAction("UserBankAccounts", "Payment");
                        }
                        else
                        {
                            ViewBag.bankAccRestEditMsg = "Failed to add";
                            BindBankNames();
                            return View();
                        }
                    }
                    BindBankNames();
                    return View();
                }
                else
                {
                    BindBankNames();
                    return View();
                }
                
            }
            else
            {
                if (Session["Customer"] != null)
                {
                    tbl_UserBankAcc insObj = new tbl_UserBankAcc();
                    if (ModelState.IsValid)
                    {
                        tbl_Customer cusObj = cusMngr.GetCustomerDetailsByEmailId(Session["Customer"].ToString());
                        insObj.AccNumber = obj.AccNumber;
                        insObj.Branch = obj.Branch;
                        insObj.IfscCode = obj.IfscCode;
                        insObj.User_fk_BankName = obj.fk_BankName;
                        insObj.rder_fk_CusId = cusObj.CusId;

                        int status = payMngr.AddOrEditUserBankAccounts(insObj);
                        if (status > 0)
                        {
                            ViewBag.bankAccAddMsg = "Added Successfully";
                            return RedirectToAction("UserBankAccounts", "Payment");
                        }
                        else
                        {
                            ViewBag.bankAccAddMsg = "Failed to add account";
                            BindBankNames();
                            return View();
                        }
                    }
                    ViewBag.bankAccAddMsg = "Failed to add account";
                    BindBankNames();
                    return View();
                }
                else if(Session["Restaurant"]!=null)
                {
                    tbl_Restaurant restObj = restMngr.IsExistEmail(Session["Restaurant"].ToString());
                    tbl_ResBankAcc isAccExistObj = payMngr.IsExistBankAccount(restObj.RestId);
                    if (isAccExistObj == null)
                    {
                        tbl_ResBankAcc insObj = new tbl_ResBankAcc();
                        if (ModelState.IsValid)
                        {

                            insObj.AccNumber = obj.AccNumber;
                            insObj.Branch = obj.Branch;
                            insObj.IfscCode = obj.IfscCode;
                            insObj.Rest_fk_BankName = obj.fk_BankName;
                            insObj.bank_fk_RestId = restObj.RestId;

                            int status = payMngr.AddOrEditRestBankAccounts(insObj);
                            if (status > 0)
                            {
                                ViewBag.bankAccRestAddMsg = "Added Successfully";
                                return RedirectToAction("UserBankAccounts", "Payment");
                            }
                            else
                            {
                                ViewBag.bankAccRestAddMsg = "Failed to add";
                                BindBankNames();
                                return View();
                            }
                        }
                        ViewBag.bankAccRestAddMsg = "Failed to add";
                        BindBankNames();
                        return View();
                    }
                    else
                    {
                        ViewBag.bankAccRestAddMsg = "Bank account already exists. You can only add one bank account";
                        BindBankNames();
                        return View();
                    }
                    
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
                

            }


        }

        public void BindBankNames(int id = 0)
        {
            List<SelectListItem> ddl_Banks = new List<SelectListItem>();
            List<tbl_BankNames> retbanklist = payMngr.GetAllBankNames();
            List<BankIdModel> disbankList = new List<BankIdModel>();

            foreach (var item in retbanklist)
            {
                disbankList.Add(new BankIdModel
                {
                    BankId = item.BankId,
                    BankName = item.BankName
                });
            }
            if (id <= 0)
            {
                ddl_Banks.Add(new SelectListItem
                {
                    Selected = true,
                    Text = "-- Select Your Bank --",
                    Value = ""
                });
                foreach (var item in disbankList)
                {
                    ddl_Banks.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = item.BankName,
                        Value = item.BankId.ToString()
                    });
                }
            }
            else
            {
                foreach (var item in disbankList)
                {
                    if (id != item.BankId)
                    {
                        ddl_Banks.Add(new SelectListItem
                        {
                            Selected = false,
                            Text = item.BankName,
                            Value = item.BankId.ToString()
                        });

                    }
                    else
                    {
                        ddl_Banks.Add(new SelectListItem
                        {
                            Selected = true,
                            Text = item.BankName,
                            Value = item.BankId.ToString()
                        });
                    }

                }

            }


            TempData["ddlBankList"] = ddl_Banks;

        }

        public ActionResult DeleteBankAcc(int? id)
        {
            if (Session["Customer"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                tbl_UserBankAcc retBankObj = payMngr.GetBankAccountById(Convert.ToInt32(id));
                BankAccounts disBankObj = new BankAccounts();
                disBankObj.AccNumber = retBankObj.AccNumber;
                disBankObj.IfscCode = retBankObj.IfscCode;
                disBankObj.Branch = retBankObj.Branch;

                return View(disBankObj);

            }
            else if (Session["Restaurant"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                tbl_ResBankAcc retRestBankObj = payMngr.GetRestBankAccountById(Convert.ToInt32(id));
                BankAccounts disBankObj = new BankAccounts();
                disBankObj.AccNumber = retRestBankObj.AccNumber;
                disBankObj.IfscCode = retRestBankObj.IfscCode;
                disBankObj.Branch = retRestBankObj.Branch;

                return View(disBankObj);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }


        [HttpPost]
        [ActionName("DeleteBankAcc")]
        public ActionResult DeleteBankAccout(int? id)
        {
            if (Session["Customer"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                int result = payMngr.RemovebankAccout(Convert.ToInt32(id));
                if (result > 0)
                {
                    return RedirectToAction("UserBankAccounts", "Payment");
                }
                else
                {
                    ViewBag.delMsg = "Failed to delete";
                    return View();
                }
            }
            else if (Session["Restaurant"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                int result = payMngr.RemoveRestbankAccout(Convert.ToInt32(id));
                if (result > 0)
                {
                    return RedirectToAction("UserBankAccounts", "Payment");
                }
                else
                {
                    ViewBag.delMsg = "Failed to delete";
                    return View();
                }

            }
            else
            {
                ViewBag.delMsg = "Failed to delete";
                return View();
            }
           

        } 

        [HttpPost]
        public ActionResult MakePayment(BankAccounts obj) 
        {
            if(obj != null)
            {
                if (Session["OrderItem"] != null)
                {
                    tbl_UserBankAcc bankObj = new tbl_UserBankAcc();
                    
                    bankObj.id = obj.AccId;
                    string pinNum = obj.PinNumber;
                    tbl_OrderDetails payObj = (tbl_OrderDetails)Session["OrderItem"];
                    string result = payMngr.PaymentTransaction(bankObj, payObj, pinNum);
                    if (result == "Success")
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else if (result == "Mismatch")
                    {
                        return Json("Invalid Pin Number", JsonRequestBehavior.AllowGet);
                    }
                    else if(result=="Account not found")
                    {
                        return Json("Sorry.. invalid bank account", JsonRequestBehavior.AllowGet);
                    }
                    else if(result=="Destination acc not found")
                    {
                        return Json("Destination account not found", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed due to technical error", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Session expired", JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                return Json("Failed due to technical error", JsonRequestBehavior.AllowGet);

            }
        }

    }
}