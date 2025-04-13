using ECommerceApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number.")]
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        [AllowedFileExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        [MaxFileSize(10 * 1024 * 1024)]
        public IFormFile? ImageFile { get; set; }
    }
}
