﻿namespace OnlineBookStore.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
