﻿namespace ECommerceApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
