using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceApp.Migrations
{
    /// <inheritdoc />
    public partial class seedData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 13, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 14, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 15, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 16, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 17, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 18, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 19, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 20, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 21, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 },
                    { 22, 5, "Non-slip yoga mat for comfortable practice", "/images/products/yoga-mat.jpg", "Yoga Mat", 24.99m, 70 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);
        }
    }
}
