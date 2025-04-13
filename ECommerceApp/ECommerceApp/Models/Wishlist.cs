using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
