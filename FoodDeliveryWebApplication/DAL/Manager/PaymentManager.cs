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
        public List<tbl_ResBankAcc> GetAllBankAccountsofRest(string restEmail)
        {
            return db.tbl_ResBankAcc.Where(e => e.tbl_Restaurant.RestEmail == restEmail).ToList();
        }
        public List<tbl_BankNames> GetAllBankNames()
        {
            return db.tbl_BankNames.ToList();
        }
        public tbl_UserBankAcc GetBankAccountById(int bankId)
        {
            return db.tbl_UserBankAcc.Where(e => e.id == bankId).SingleOrDefault();
        }
     
        public tbl_ResBankAcc GetRestBankAccountById(int bankId)
        {
            return db.tbl_ResBankAcc.Where(e => e.id == bankId).SingleOrDefault();
        }
        public int AddOrEditUserBankAccounts(tbl_UserBankAcc obj)
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
        public int AddOrEditRestBankAccounts(tbl_ResBankAcc obj)
        {
            if (obj.id > 0)
            {
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
            else
            {
                db.tbl_ResBankAcc.Add(obj);
                return db.SaveChanges();
            }
        }

        public int RemovebankAccout(int? id)
        {
            tbl_UserBankAcc remObj = db.tbl_UserBankAcc.Find(id);
            db.tbl_UserBankAcc.Remove(remObj);
            return db.SaveChanges();
        }

        public int RemoveRestbankAccout(int? id)
        {
            tbl_ResBankAcc remObj = db.tbl_ResBankAcc.Find(id);
            db.tbl_ResBankAcc.Remove(remObj);
            return db.SaveChanges();
        }

        public string PaymentTransaction(tbl_UserBankAcc obj, tbl_OrderDetails payObj,string pinNumber)
        {
            tbl_UserBankAcc checkObj = db.tbl_UserBankAcc.Where(e => e.id == obj.id).SingleOrDefault();
            if (checkObj != null)
            {

                tbl_BankAccounts comBankObj = db.tbl_BankAccounts.Where(e => e.AccNumber == checkObj.AccNumber && e.Branch == checkObj.Branch && e.IfscCode == checkObj.IfscCode && e.fk_BankName == checkObj.User_fk_BankName).SingleOrDefault();
                if (comBankObj != null)
                {
                    if (comBankObj.PinNumber == pinNumber)
                    {
                        tbl_ResBankAcc restBankObj = db.tbl_ResBankAcc.Where(e => e.bank_fk_RestId == payObj.Order_fk_RestId).SingleOrDefault();
                        if (restBankObj != null)
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            SqlCommand cmd = new SqlCommand("sp_Payment", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CusAccNum", checkObj.AccNumber));
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

        public tbl_ResBankAcc IsExistBankAccount(int? restId)
        {
            return db.tbl_ResBankAcc.Where(e => e.bank_fk_RestId == restId).SingleOrDefault();
        }


    }
   
}
