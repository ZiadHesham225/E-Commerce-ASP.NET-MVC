using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.ViewModels;

namespace ECommerceApp.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderWithOrderDetails(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersWithUsersAndOrderDetailsAsync();
        Task<IEnumerable<OrderUserViewModel>> GetAllOrdersWithUsersAsync();
    }
}
