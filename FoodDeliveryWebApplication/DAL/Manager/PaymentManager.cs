using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{   
    public class PaymentManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-SRG4EAKH;Initial Catalog=db_FoodOrderingApplication;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
        public List<tbl_UserBankAcc> GetAllBankAccountsofUser(string userEmail)
        {
            return db.tbl_UserBankAcc.Where(e =>e.tbl_Customer.CusEmail == userEmail).ToList();
        }
        public List<tbl_BankNames> GetAllBankNames()
        {
            return db.tbl_BankNames.ToList();
        }
        public tbl_UserBankAcc GetBankAccountById(int bankId)
        {
            return db.tbl_UserBankAcc.Where(e => e.id == bankId).SingleOrDefault();
        }
        public int AddOrEditAccounts(tbl_UserBankAcc obj)
        {
            if (obj.id > 0)
            {
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
            else
            {
                db.tbl_UserBankAcc.Add(obj);
                return db.SaveChanges();
            }
        }

        public int RemovebankAccout(int? id)
        {
            tbl_UserBankAcc remObj = db.tbl_UserBankAcc.Find(id);
            db.tbl_UserBankAcc.Remove(remObj);
            return db.SaveChanges();
        }

        public string PaymentTransaction(tbl_UserBankAcc obj, tbl_OrderDetails payObj,string pinNumber)
        {
            tbl_UserBankAcc checkObj = db.tbl_UserBankAcc.Where(e => e.AccNumber == obj.AccNumber).SingleOrDefault();
            if (checkObj != null)
            {

                tbl_BankAccounts comBankObj = db.tbl_BankAccounts.Where(e => e.AccNumber == checkObj.AccNumber && e.Branch == checkObj.Branch && e.IfscCode == checkObj.IfscCode && e.fk_BankName == checkObj.User_fk_BankName).SingleOrDefault();
                if (comBankObj != null)
                {
                    if (comBankObj.PinNumber == pinNumber)
                    {
                        tbl_UserBankAcc restBankObj = db.tbl_UserBankAcc.Where(e => e.rder_fk_CusId == payObj.Order_fk_RestId).SingleOrDefault();
                        if (restBankObj != null)
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            SqlCommand cmd = new SqlCommand("sp_Payment", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CusAccNum", obj.AccNumber));
                            cmd.Parameters.Add(new SqlParameter("@RestAccNum", restBankObj.AccNumber));
                            cmd.Parameters.Add(new SqlParameter("@amount", payObj.TotalAmount));
                            string result = cmd.ExecuteScalar().ToString();
                            con.Close();
                            if (result == "success")
                            {
                                return "Success";
                            }
                            else
                            {
                                return "Failed";
                            }
                           

                        }
                        else
                        {
                            return "Destination acc not found";
                        }


                    }
                    
                    
                    else
                    {
                       return "Mismatch";
                    }
                }
                else
                {
                    return "Account not found";
                }
            }
            else
            {
                return "Not found";
            }

            
        }


    }
   
}
