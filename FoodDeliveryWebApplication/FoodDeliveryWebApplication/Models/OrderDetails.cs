using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FoodDeliveryWebApplication.Models
{
    public class OrderDetails
    {
        [Key]
        public int id { get; set; }
        public Nullable<int> fk_OrderId { get; set; }
        [DisplayName("Food Detais")]
        public Nullable<int> fk_DishId { get; set; }
        public Nullable<int> DishQuantity { get; set; }
        [DisplayName("Customer Name")]
        public string CusName { get; set; }
        public string RestName { get; set; }
        public Nullable<int> Order_fk_CusId { get; set; }
        [Required]
        public Nullable<int> Order_fk_AddId { get; set; }
        public Nullable<int> Order_fk_RestId { get; set; }
        public Nullable<int> Order_fk_StaffId { get; set; }
        [Required]
        [DisplayName("Bill Amount")]
        public Nullable<decimal> TotalAmount { get; set; }
        [DisplayName("Order Date")]
        public Nullable<System.DateTime> Orderdate { get; set; }
        [Required]
        public string PaymentMode { get; set; }
        public string Image { get; set; }
        public string IsPicked { get; set; }
        public string IsCancelled { get; set; }
        public string IsPaid { get; set; }
        public string IsDelivered { get; set; }
        [DisplayName("Order Confirmation")]
        public string IsOrderConfirmed { get; set; }
        public string OrderOtp { get; set; }
        [DisplayName("Ordered Food Items")]
        public virtual ICollection<OrderedFoodItems> tbl_OrderedFoodDetails { get; set; }
        public ICollection<Dishes> FoodDetails { get; set; }
        public string DoorOrFlatNo { get; set; }

        public string LandMark { get; set; }

        public int? AddressType { get; set; }

        public string PinCode { get; set; }
        public int? Add_fk_CusId { get; set; }

        public string AddressTypeName { get; set; }

        public IList<Cart> CartDetails { get; set; }
    }
}