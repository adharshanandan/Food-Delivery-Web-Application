using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Data.Entity;

namespace DAL.Manager
{
    public class AddressManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public tbl_Addresses GetAddressById(int id)
        {
            return db.tbl_Addresses.Where(e => e.AddId == id).FirstOrDefault();
        }
        public string  InsertAddress(tbl_Addresses insObj, string cusEmailId="")
        {
            if (insObj.AddId > 0)
            {
                tbl_Addresses editObj = db.tbl_Addresses.Where(e => e.AddId == insObj.AddId).SingleOrDefault();
                editObj.AddressType = insObj.AddressType;
                editObj.DoorOrFlatNo = insObj.DoorOrFlatNo;
                editObj.PinCode = insObj.PinCode;
                editObj.LandMark = insObj.LandMark;
                db.Entry(editObj).State = EntityState.Modified;
                int status = db.SaveChanges();
                if (status > 0)
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
                var cusId = (from p in db.tbl_Customer where p.CusEmail.Contains(cusEmailId) select p.CusId).ToArray();
                insObj.Add_fk_CusId = Convert.ToInt32(cusId[0]);
                db.tbl_Addresses.Add(insObj);
                int status = db.SaveChanges();
                if (status > 0)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            
        }

        public List<tbl_Addresses> GetAllAddresses(string cusEmailId)
        {
            return db.tbl_Addresses.Where(e => e.tbl_Customer.CusEmail == cusEmailId).ToList();
        }

        public List<tbl_AddressType> GetAddressTypes()
        {
            return db.tbl_AddressType.ToList();
        }
        public string DeleteAddress(int? id)
        {
            tbl_Addresses remObj = db.tbl_Addresses.Where(e => e.AddId == id).SingleOrDefault();
            if (remObj == null)
            {
                return "Failed";
            }
            db.tbl_Addresses.Remove(remObj);
            int status = db.SaveChanges();
            if (status > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }
        public string DeletePhone(int? id)
        {
            tbl_PhoneNumbers remObj = db.tbl_PhoneNumbers.Where(e => e.PhoneId == id).SingleOrDefault();
            if (remObj == null)
            {
                return "Failed";
            }
            db.tbl_PhoneNumbers.Remove(remObj);
            int status = db.SaveChanges();
            if (status > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        public string  InsertPhone(tbl_PhoneNumbers insObj,string cusEmailId)
        {
            var cusId = (from p in db.tbl_Customer where p.CusEmail.Contains(cusEmailId) select p.CusId).ToArray();
            insObj.Phn_fk_CusId = Convert.ToInt32(cusId[0]);
            tbl_PhoneNumbers checkObj = db.tbl_PhoneNumbers.Where(e => e.PhoneNumbers == insObj.PhoneNumbers && e.Phn_fk_CusId == insObj.Phn_fk_CusId).SingleOrDefault();
            if (checkObj != null)
            {
                return "Exist";
            }
            db.tbl_PhoneNumbers.Add(insObj);
            int status = db.SaveChanges();
            if (status > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }

        }
        public string PicodeCheckForDelivery(int? id)
        {
            tbl_Addresses retObj = db.tbl_Addresses.Where(e => e.AddId == id).SingleOrDefault();
            return retObj.PinCode;
        }
    }
}
