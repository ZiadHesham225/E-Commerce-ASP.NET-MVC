using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Clothing" },
                    { 3, "Books" },
                    { 4, "Home & Garden" },
                    { 5, "Sports & Outdoors" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "High-performance laptop with the latest CPU and GPU", "/images/products/laptop.jpg", "Laptop", 1299.99m, 50 },
                    { 2, 1, "Latest smartphone with advanced camera and long battery life", "/images/products/smartphone.jpg", "Smartphone", 899.99m, 100 },
                    { 3, 1, "Noise-cancelling wireless headphones", "/images/products/headphones.jpg", "Headphones", 249.99m, 75 },
                    { 4, 2, "Comfortable cotton t-shirt", "/images/products/tshirt.jpg", "T-Shirt", 19.99m, 200 },
                    { 5, 2, "Classic denim jeans", "/images/products/jeans.jpg", "Jeans", 49.99m, 150 },
                    { 6, 2, "Warm hoodie for cold weather", "/images/products/hoodie.jpg", "Hoodie", 39.99m, 100 },
                    { 7, 3, "Learn programming fundamentals", "/images/products/programming-book.jpg", "Programming Basics", 29.99m, 60 },
                    { 8, 3, "Bestselling science fiction novel", "/images/products/scifi-book.jpg", "Sci-Fi Novel", 14.99m, 80 },
                    { 9, 4, "Modern coffee table for your living room", "/images/products/coffee-table.jpg", "Coffee Table", 199.99m, 30 },
                    { 10, 4, "Complete set of essential garden tools", "/images/products/garden-tools.jpg", "Garden Tools Set", 79.99m, 40 },
                    { 11, 5, "Official size basketball", "/images/products/basketball.jpg", "Basketball", 29.99m, 50 },
                    { 12, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
