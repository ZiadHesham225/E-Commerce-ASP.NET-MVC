using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Repositories;
using ECommerceApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ECommerceApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        public async Task<Order> CreateOrder(Order order)
        {
            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();
            return order;
        }
        public async Task DeleteOrder(int orderId)
        {
            await _orderRepository.DeleteAsync(orderId);
            await _orderRepository.SaveAsync();
        }
        public async Task<Order> PlaceOrderAsync(Order order)
        {
            foreach (var item in order.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException("Product is not available or stock is insufficient.");
                }
                product.Stock -= item.Quantity;

            }

            order.TotalPrice = order.OrderDetails.Sum(d => d.Quantity * d.Price);
            Order newOrder = await _orderRepository.CreateAsync(order);
            return newOrder;
        }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
        public async Task<Order> GetOrderAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return false;
            }
            order.Status = status;
             _orderRepository.Update(order);
            await _orderRepository.SaveAsync();
            return true;
        }
        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithOrderDetails(orderId);
            if (order == null || order.Status == "Shipped" || order.Status == "Delivered") return false;

            foreach (var item in order.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }
            }

            order.Status = "Canceled";
            _orderRepository.Update(order);
            await _orderRepository.SaveAsync();
            return true;
        }
        public async Task<decimal> GetRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = await _orderRepository.GetAllAsync();

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate.Value);
            }

            return query.Sum(o => o.TotalPrice);
        }
        public async Task SaveAsync()
        {
            await _orderRepository.SaveAsync();
        }
        public async Task<int> GetPendingOrdersCount()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Where(o => o.Status == "Pending").Count();
        }
        public async Task<int> GetOrdersCountByDate(DateTime? date = null)
        {
            date ??= DateTime.Today;
            var orders = await _orderRepository.GetAllAsync();
            return orders.Count(o => o.OrderDate.Date == date.Value.Date);
        }
        public async Task<IEnumerable<OrderDto>> GetRecentOrders(int count)
        {
            var recentOrders = await _orderRepository.GetOrdersWithUsersAndOrderDetailsAsync();

            return recentOrders
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToList();
        }
        public async Task<ChartData> GetSalesDataAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var sales = orders
                .Where(o => o.OrderDate >= DateTime.Now.AddMonths(-5))
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month)}",
                    TotalSales = g.Sum(o => o.TotalPrice)
                })
                .OrderBy(g => g.Month)
                .ToList();

            return new ChartData
            {
                Months = sales.Select(s => s.Month).ToList(),
                Values = sales.Select(s => (int)s.TotalSales).ToList()
            };
        }
        public async Task<ChartData> GetOrdersDataAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var ordersData = orders
                .Where(o => o.OrderDate >= DateTime.Now.AddDays(-7))
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            return new ChartData
            {
                Months = ordersData.Select(o => o.Date.ToString("ddd")).ToList(),
                Values = ordersData.Select(o => o.OrderCount).ToList()
            };
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetOrdersWithUsersAndOrderDetailsAsync();
        }
        public async Task<IEnumerable<OrderUserViewModel>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _orderRepository.GetAllOrdersWithUsersAsync();
            return orders.Where(o => o.Status == status);
        }
    }
}
