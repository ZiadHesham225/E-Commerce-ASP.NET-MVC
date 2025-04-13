using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<User> userManager)
        {
            var adminUser = new User
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FullName = "Admin User"
            };

            var customerUser = new User
            {
                UserName = "customer@example.com",
                Email = "customer@example.com",
                EmailConfirmed = true,
                FullName = "Customer User"
            };

            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            if (userManager.Users.All(u => u.UserName != customerUser.UserName))
            {
                await userManager.CreateAsync(customerUser, "Customer123!");
                await userManager.AddToRoleAsync(customerUser, "Customer");
            }
        }

        public static void SeedDatabase(ModelBuilder builder)
        {
            // Seed Categories
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" },
                new Category { Id = 4, Name = "Home & Garden" },
                new Category { Id = 5, Name = "Sports & Outdoors" }
            };
            builder.Entity<Category>().HasData(categories);

            // Seed Products
            var products = new List<Product>
            {
                new Product {
                    Id = 1,
                    Name = "Laptop",
                    Description = "High-performance laptop with the latest CPU and GPU",
                    Price = 1299.99m,
                    Stock = 50,
                    ImageUrl = "/images/products/laptop.jpg",
                    CategoryId = 1
                },
                new Product {
                    Id = 2,
                    Name = "Smartphone",
                    Description = "Latest smartphone with advanced camera and long battery life",
                    Price = 899.99m,
                    Stock = 100,
                    ImageUrl = "/images/products/smartphone.jpg",
                    CategoryId = 1
                },
                new Product {
                    Id = 3,
                    Name = "Headphones",
                    Description = "Noise-cancelling wireless headphones",
                    Price = 249.99m,
                    Stock = 75,
                    ImageUrl = "/images/products/headphones.jpg",
                    CategoryId = 1
                },
                new Product {
                    Id = 4,
                    Name = "T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 19.99m,
                    Stock = 200,
                    ImageUrl = "/images/products/tshirt.jpg",
                    CategoryId = 2
                },
                new Product {
                    Id = 5,
                    Name = "Jeans",
                    Description = "Classic denim jeans",
                    Price = 49.99m,
                    Stock = 150,
                    ImageUrl = "/images/products/jeans.jpg",
                    CategoryId = 2
                },
                new Product {
                    Id = 6,
                    Name = "Hoodie",
                    Description = "Warm hoodie for cold weather",
                    Price = 39.99m,
                    Stock = 100,
                    ImageUrl = "/images/products/hoodie.jpg",
                    CategoryId = 2
                },
                new Product {
                    Id = 7,
                    Name = "Programming Basics",
                    Description = "Learn programming fundamentals",
                    Price = 29.99m,
                    Stock = 60,
                    ImageUrl = "/images/products/programming-book.jpg",
                    CategoryId = 3
                },
                new Product {
                    Id = 8,
                    Name = "Sci-Fi Novel",
                    Description = "Bestselling science fiction novel",
                    Price = 14.99m,
                    Stock = 80,
                    ImageUrl = "/images/products/scifi-book.jpg",
                    CategoryId = 3
                },
                new Product {
                    Id = 9,
                    Name = "Coffee Table",
                    Description = "Modern coffee table for your living room",
                    Price = 199.99m,
                    Stock = 30,
                    ImageUrl = "/images/products/coffee-table.jpg",
                    CategoryId = 4
                },
                new Product {
                    Id = 10,
                    Name = "Garden Tools Set",
                    Description = "Complete set of essential garden tools",
                    Price = 79.99m,
                    Stock = 40,
                    ImageUrl = "/images/products/garden-tools.jpg",
                    CategoryId = 4
                },
                new Product {
                    Id = 11,
                    Name = "Basketball",
                    Description = "Official size basketball",
                    Price = 29.99m,
                    Stock = 50,
                    ImageUrl = "/images/products/basketball.jpg",
                    CategoryId = 5
                },
                new Product {
                    Id = 12,
                    Name = "Yoga Mat",
                    Description = "Non-slip yoga mat for comfortable practice",
                    Price = 24.99m,
                    Stock = 70,
                    ImageUrl = "/images/products/yoga-mat.jpg",
                    CategoryId = 5
                }
            };
            builder.Entity<Product>().HasData(products);
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole("Customer"));

            await SeedDefaultUserAsync(userManager);

            // Get customer user
            var customer = await userManager.FindByEmailAsync("customer@example.com");

            if (customer != null && !context.Carts.Any())
            {
                // Create cart for customer
                var cart = new Cart
                {
                    UserId = customer.Id
                };
                context.Carts.Add(cart);
                await context.SaveChangesAsync();

                // Add some items to cart
                var cartItems = new List<CartItem>
                {
                    new CartItem { CartId = cart.Id, ProductId = 1, Quantity = 1 },
                    new CartItem { CartId = cart.Id, ProductId = 4, Quantity = 2 }
                };
                context.CartItems.AddRange(cartItems);

                // Create sample orders
                var order = new Order
                {
                    UserId = customer.Id,
                    OrderDate = DateTime.Now.AddDays(-5),
                    TotalPrice = 1339.97m,
                    Status = "Delivered"
                };
                context.Orders.Add(order);
                await context.SaveChangesAsync();

                // Add order details
                var orderDetails = new List<OrderDetail>
                {
                    new OrderDetail { OrderId = order.Id, ProductId = 1, Quantity = 1, Price = 1299.99m },
                    new OrderDetail { OrderId = order.Id, ProductId = 3, Quantity = 1, Price = 39.98m }
                };
                context.OrderDetails.AddRange(orderDetails);

                // Add a review
                var review = new Review
                {
                    UserId = customer.Id,
                    ProductId = 1,
                    Rating = 5,
                    Comment = "Excellent laptop, very fast and reliable.",
                    CreatedAt = DateTime.Now.AddDays(-3)
                };
                context.Reviews.Add(review);

                // Add a wishlist item
                var wishlistItem = new Wishlist
                {
                    UserId = customer.Id,
                    ProductId = 2
                };
                context.Wishlists.Add(wishlistItem);

                // Add a transaction
                var transaction = new Transaction
                {
                    UserId = customer.Id,
                    OrderId = order.Id,
                    Amount = order.TotalPrice,
                    Type = "Purchase",
                    Date = order.OrderDate
                };
                context.Transactions.Add(transaction);

                await context.SaveChangesAsync();
            }
        }
    }
}