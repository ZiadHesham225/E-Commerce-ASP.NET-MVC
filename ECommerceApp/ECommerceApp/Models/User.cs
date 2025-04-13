using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
