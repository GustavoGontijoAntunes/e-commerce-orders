﻿namespace eCommerceOrders.SendEmail.Models
{
    public class Product
    {
        public Product() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
    }
}