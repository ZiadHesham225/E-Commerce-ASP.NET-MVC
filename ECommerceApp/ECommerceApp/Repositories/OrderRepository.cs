using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await dbSet.Where(o => o.UserId == userId).Include(o => o.OrderDetails).ThenInclude(od => od.Product).OrderByDescending(o=> o.OrderDate).ToListAsync();
        }
        public async Task<Order> GetOrderWithOrderDetails(int orderId)
        {
            return await dbSet.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<IEnumerable<OrderDto>> GetOrdersWithUsersAndOrderDetailsAsync()
        {
            return await dbSet.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(od => od.Product).Select(o => new OrderDto
            {
                Id = o.Id,
                Status = o.Status,
                TotalPrice = o.TotalPrice,
                OrderDate = o.OrderDate,
                CustomerName = o.User.FullName ?? "Deleted User",
                orderDetails = o.OrderDetails
            }).ToListAsync();
        }
        public async Task<IEnumerable<OrderUserViewModel>> GetAllOrdersWithUsersAsync()
        {
            return await dbSet.Include(o => o.User).Select(o => new OrderUserViewModel
            {
                Id = o.Id,
                Status = o.Status,
                CustomerName = o.User.FullName ?? "Deleted User"
            }).ToListAsync();
        }
    }
}
