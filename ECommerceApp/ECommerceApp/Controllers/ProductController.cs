using ECommerceApp.DTOs;
using ECommerceApp.Repositories;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICategoryRepository _categoryRepository;
        private const int PageSize = 12;

        public ProductController(
            ICartService cartService,
            IProductService productService,
            ICategoryRepository categoryRepository)
        {
            _cartService = cartService;
            _productService = productService;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index(ProductFilterDto filter, int page = 1)
        {
            var isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            var allFilteredProducts = await _productService.GetFilteredProductsAsync(filter);
            var paginatedProducts = _productService.GetPaginatedProducts(allFilteredProducts, page, PageSize);
            Dictionary<int, int> cartQuantities = new Dictionary<int, int>();
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var product in paginatedProducts)
                {
                    int quantity = await _cartService.GetProductQuantityInCart(userId, product.Id);
                    cartQuantities[product.Id] = quantity;
                }
            }
            ViewBag.CartQuantities = cartQuantities;
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.categories = categories;

            var totalProducts = allFilteredProducts.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);
            ViewBag.totalPages = totalPages;
            ViewBag.currentPage = page;

            return View(paginatedProducts);
        }
        public async Task<IActionResult> Details(int id)
        {
            int cartQuantity = 0;
            var product = await _productService.GetProductWithCategoryAndReviewsByIdAsync(id);
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cartQuantity = await _cartService.GetProductQuantityInCart(userId, id);
            }
            ViewBag.CartQuantity = cartQuantity;
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            if (!ModelState.IsValid) return View(productDto);
            await _productService.AddProductAsync(productDto);
            return RedirectToAction("adminDisplay");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            return View(product);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ProductDto productDto)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            if (!ModelState.IsValid) return View(productDto);
            await _productService.UpdateProductAsync(productDto);
            return RedirectToAction("adminDisplay");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("adminDisplay");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> adminDisplay(int pageNumber = 1, int pageSize = 15)
        {
            var products = await _productService.GetProductsAsync(pageNumber, pageSize);
            return View(products);
        }
        public async Task<IActionResult> adminDetails(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return View(product);
        }
    }
}
