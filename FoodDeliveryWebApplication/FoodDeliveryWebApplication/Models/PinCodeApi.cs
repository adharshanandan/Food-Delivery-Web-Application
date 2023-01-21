using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDeliveryWebApplication.Models
{
    public class PostOffice
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BranchType { get; set; }
        public string DeliveryStatus { get; set; }
        public string Circle { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }
        public string Block { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
    }
    public class PincodeProps
    {
        public string Message { get; set; }
        public string Status { get; set; }

        public IList<PostOffice> PostOffice { get; set; }
    }
}