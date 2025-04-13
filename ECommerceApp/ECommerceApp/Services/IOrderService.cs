using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.ViewModels;

namespace ECommerceApp.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<decimal> GetRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetPendingOrdersCount();
        Task<int> GetOrdersCountByDate(DateTime? date = null);
        Task<IEnumerable<OrderDto>> GetRecentOrders(int count);
        Task<ChartData> GetSalesDataAsync();
        Task<ChartData> GetOrdersDataAsync();
        Task SaveAsync();
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderUserViewModel>> GetOrdersByStatusAsync(string status);
    }
}
