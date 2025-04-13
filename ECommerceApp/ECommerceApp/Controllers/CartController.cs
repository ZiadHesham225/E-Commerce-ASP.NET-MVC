using ECommerceApp.Repositories;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartController(ICartService cartService, ICartItemRepository cartItemRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _cartService = cartService;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return View(cartItems);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            bool result = await _cartService.AddToCartAsync(userId, productId, quantity);
            if(!result)
            {
                TempData["CartErrorMessage"] = "Product is out of stock.";
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int productId, string operation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null)
            {
                return NotFound();
            }

            if (operation == "increment")
            {
                await _cartService.UpdateCartItemQuantityAsync(userId, productId, cartItem.Quantity + 1);
            }
            else if (operation == "decrement" && cartItem.Quantity > 1)
            {
                await _cartService.UpdateCartItemQuantityAsync(userId, productId, cartItem.Quantity - 1);
            }
            cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId);
            decimal itemTotal = cartItem.Quantity * cartItem.Product.Price;
            decimal cartTotal = cart.CartItems.Sum(i => i.Quantity * i.Product.Price);
            return Json(new
            {
                quantity = cartItem.Quantity,
                itemTotal = itemTotal.ToString("C"),
                cartTotal = cartTotal.ToString("C"),
                Stock = cartItem.Product.Stock
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            await _cartService.RemoveFromCartAsync(userId, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            await _cartService.ClearCartAsync(userId);
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            int count = cart?.ItemCount ?? 0;

            return Json(new { count });
        }
    }
}