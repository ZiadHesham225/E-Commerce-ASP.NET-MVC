using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation Properties
        public Category? Category { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public static implicit operator List<object>(Product v)
        {
            throw new NotImplementedException();
        }
    }
}
