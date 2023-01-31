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
        public ActionResult SelectBankToPay(decimal payAmount)
        {
            BindBankAccounts(Session["Customer"].ToString());
            UserAddedBankAccounts obj = new UserAddedBankAccounts();
            return View(obj);
        }
        public void BindBankAccounts(string EmailId)
        {
            List<SelectListItem> ddl_UserBanks = new List<SelectListItem>();
            List<tbl_UserBankAcc> _list = payMngr.GetAllBankAccountsofUser(EmailId);
            List<UserAddedBankAccounts> disList = new List<UserAddedBankAccounts>();
            foreach(var item in _list)
            {
                disList.Add(new UserAddedBankAccounts
                {
                    AccId = item.id,                    
                    AccNumber=item.AccNumber
                }) ;
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
    }
}