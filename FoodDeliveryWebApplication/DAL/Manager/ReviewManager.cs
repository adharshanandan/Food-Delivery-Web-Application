using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Manager
{
    public class ReviewManager
    {
        db_FoodOrderingApplicationEntities db = new db_FoodOrderingApplicationEntities();
        public int PostReviews(tbl_ReviewAndRating insObj)
        {
            db.tbl_ReviewAndRating.Add(insObj);
            return db.SaveChanges();
        }

        public List<tbl_ReviewAndRating> GetReviewListByRestId(int id)
        {
            return db.tbl_ReviewAndRating.Where(e => e.Rev_fk_RestId == id).ToList();
        }
    }
}
