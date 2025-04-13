using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }
        public int ItemCount { get; set; } = 0;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}
