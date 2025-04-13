using ECommerceApp.Models;

namespace ECommerceApp.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public Decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();
    }
}
