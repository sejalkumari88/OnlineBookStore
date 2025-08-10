using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; }

        public string PaymentMethod { get; set; }  // COD or Online
        public string Status { get; set; } = "Pending";

        // Razorpay specific
        public string RazorpayOrderId { get; set; }
        public string RazorpayPaymentId { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
