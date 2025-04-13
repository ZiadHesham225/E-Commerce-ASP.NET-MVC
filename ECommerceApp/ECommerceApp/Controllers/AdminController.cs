using ECommerceApp.Models;
using ECommerceApp.Services;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        public AdminController(UserManager<User> userManager, IOrderService orderService, IProductService productService)
        {
            _userManager = userManager;
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var totalRevenue = await _orderService.GetRevenueAsync();
            var pendingOrders = await _orderService.GetPendingOrdersCount();
            var todayOrders = await _orderService.GetOrdersCountByDate();
            var topSellingProducts = await _productService.GetTopSellingProducts(5);
            var recentOrders = await _orderService.GetRecentOrders(5);
            var salesData = await _orderService.GetSalesDataAsync();
            var ordersData = await _orderService.GetOrdersDataAsync();
            var model = new DashboardViewModel
            {
                TotalUsers = totalUsers,
                Revenue = totalRevenue,
                PendingOrders = pendingOrders,
                OrdersToday = todayOrders,
                TopSellingProducts = topSellingProducts,
                RecentOrders = recentOrders,
                SalesData = salesData,
                OrdersData = ordersData
            };
            return View(model);
        }

    }
}
