using ECommerceApp.DTOs;

namespace ECommerceApp.ViewModels
{
    public class ChartData
    {
        public List<string> Months { get; set; }
        public List<int> Values { get; set; }
    }
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int OrdersToday { get; set; }
        public Decimal Revenue { get; set; }
        public int PendingOrders { get; set; }
        public IEnumerable<ProductSalesDto> TopSellingProducts { get; set; } = new List<ProductSalesDto>();
        public IEnumerable<OrderDto> RecentOrders { get; set; } = new List<OrderDto>();
        public ChartData SalesData { get; set; }
        public ChartData OrdersData { get; set; }
    }
}
