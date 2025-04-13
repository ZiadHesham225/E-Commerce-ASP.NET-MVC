using ECommerceApp.Models;
using ECommerceApp.Repositories;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly ITransactionRepository _transactionRepository;

        public CheckoutController(ICartService cartService, IOrderService orderService, IPaymentService paymentService, ITransactionRepository transactionRepository, IProductRepository productRepository)
        {
            _cartService = cartService;
            _orderService = orderService;
            _paymentService = paymentService;
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartService.GetCartItemsAsync(userId);

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            foreach(var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    return RedirectToAction("Error", "Home", new { message = "Product not found" });
                }
                if (product.Stock < item.Quantity)
                {
                    return RedirectToAction("Error", "Home", new { message = $"Product {product.Name} out of stock" });
                }
            }
            var checkoutUrl = await _paymentService.CreateCheckoutSession(userId);
            return Redirect(checkoutUrl);
        }

        public async Task<IActionResult> Success(string session_id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool paymentSuccessful = await _paymentService.VerifyPaymentStatus(session_id);

            if (paymentSuccessful)
            {
                var cartItems = await _cartService.GetCartItemsAsync(userId);
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    OrderDetails = new List<OrderDetail>()
                };

                foreach (var item in cartItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        order.OrderDetails.Add(new OrderDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = product.Price
                        });
                    }
                }

                Order orderPlaced = await _orderService.PlaceOrderAsync(order);
                if (orderPlaced == null)
                {
                    return RedirectToAction("Error", "Home", new { message = "Could not place order due to inventory issues" });
                }
                await _orderService.SaveAsync();
                Transaction transaction = new Transaction
                {
                    UserId = userId,
                    OrderId = orderPlaced.Id,
                    Amount = orderPlaced.TotalPrice,
                    Type = "Payment",
                    Date = DateTime.Now,
                    Status = "Success"
                };
                await _transactionRepository.AddTransactionAsync(transaction);
                await _transactionRepository.SaveAsync();
                await _cartService.ClearCartAsync(userId);

                ViewBag.OrderId = order.Id;
                return View();
            }
            else
            {
                return RedirectToAction("Cancel");
            }
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}