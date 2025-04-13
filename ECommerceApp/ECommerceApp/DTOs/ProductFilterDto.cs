namespace ECommerceApp.DTOs
{
    public class ProductFilterDto
    {
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool InStock { get; set; } = false;
        public string? SearchQuery { get; set; }
        public string? SortBy { get; set; }
        public bool Descending { get; set; }
    }
}
