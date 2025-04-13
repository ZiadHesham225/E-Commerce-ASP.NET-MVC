using ECommerceApp.Validations;

namespace ECommerceApp.DTOs
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        [AllowedFileExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        [MaxFileSize(10 * 1024 * 1024)]
        public IFormFile? ImageFile { get; set; }
    }
}
