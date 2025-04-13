using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }

        public decimal Amount { get; set; }
        public string Type { get; set; } = "Payment";
        public string Status { get; set; } = "Pending";
        public DateTime Date { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public Order? Order { get; set; }
    }
}