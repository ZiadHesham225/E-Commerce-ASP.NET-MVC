namespace ECommerceApp.DTOs
{
    public class productWithQuantityDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public int stock { get; set; }
        public string imageUrl { get; set; }
        public int cartQuantity { get; set; }
    }
}
