using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class DeliveryBoyManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public string InsertDelGuy(tbl_DeliveryStaffs insObj)
        {

            db.tbl_DeliveryStaffs.Add(insObj);
            int result = db.SaveChanges();
            if (result > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }


        }
        public tbl_DeliveryStaffs IsExistEmail(tbl_DeliveryStaffs checkObj)
        {
            return db.tbl_DeliveryStaffs.Where(e => e.StaffEmail == checkObj.StaffEmail).SingleOrDefault();
        }

        public tbl_DeliveryStaffs IsExistPhone(tbl_DeliveryStaffs checkObj)
        {
            return db.tbl_DeliveryStaffs.Where(e => e.StaffPhone == checkObj.StaffPhone).SingleOrDefault();
        }
    }
}
