using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";

        // Navigation Properties
        public User? User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
