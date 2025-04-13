using ECommerceApp.Models;
using ECommerceApp.Services;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;
        public OrderController(IOrderService orderService,UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return View(orders);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }
        public async Task<IActionResult> OrdersByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            ViewBag.Status = status;
            return View("OrdersByStatus", orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            if (!success)
            {
                return BadRequest("Failed to update order status.");
            }
            return RedirectToAction("OrdersByStatus", new { status = newStatus == "Delivered" ? "Pending" : newStatus });
        }
    }
}
