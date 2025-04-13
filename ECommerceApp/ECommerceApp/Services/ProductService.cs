using ECommerceApp.DTOs;
using ECommerceApp.Helpers;
using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ImageService _imageService;

        public ProductService(IProductRepository productRepository, IOrderDetailRepository orderDetailRepository, ImageService imageService)
        {
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _imageService = imageService;
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductFilterDto filter)
        {
            var products = await _productRepository.GetAllAsync();

            if (filter.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.InStock)
            {
                products = products.Where(p => p.Stock > 0);
            }

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                products = products.Where(p => p.Name.Contains(filter.SearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            products = GetSortedProducts(products, filter.SortBy ?? "name", filter.Descending);

            return products;
        }

        public IEnumerable<Product> GetSortedProducts(IEnumerable<Product> products, string sortBy, bool descending = false)
        {
            products = sortBy.ToLower() switch
            {
                "name" => descending ? products.OrderByDescending(p => p.Name) : products.OrderBy(p => p.Name),
                "price" => descending ? products.OrderByDescending(p => p.Price) : products.OrderBy(p => p.Price),
                _ => products
            };

            return products;
        }

        public IEnumerable<Product> GetPaginatedProducts(IEnumerable<Product> products, int pageNumber, int pageSize)
        {
            return products.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> Search(IEnumerable<Product> products, string productName)
        {
            return products.Where(p => p.Name.Contains(productName));
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductWithCategoryAndReviewsByIdAsync(int id)
        {
            return await _productRepository.GetProductWithCategoryAndReviewsByIdAsync(id);
        }

        public async Task<IEnumerable<ProductSalesDto>> GetTopSellingProducts(int count)
        {
            var orderDetails = await _orderDetailRepository.GetAllAsync();

            var topSellingData = orderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(count)
                .ToList();

            var productIds = topSellingData.Select(p => p.ProductId).ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            return topSellingData
                .Select(p => new ProductSalesDto
                {
                    ProductId = p.ProductId,
                    ProductName = products.FirstOrDefault(prod => prod.Id == p.ProductId)?.Name ?? "Unknown",
                    TotalSales = products.FirstOrDefault(prod => prod.Id == p.ProductId)?.Price * p.TotalSold ?? 0
                })
                .ToList();
        }

        public async Task<PaginatedList<ProductDto>> GetProductsAsync(int pageNumber, int pageSize)
        {
            var query = _productRepository.GetProductsWithCategories()
        .Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId,
            CategoryName = p.CategoryName,
            ImageUrl = p.ImageUrl
        })
        .OrderBy(p => p.Name);

            return await PaginatedList<ProductDto>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProducWithCategoryByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category?.Name,
                Description = product.Description
            };
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
            };

            if (productDto.ImageFile != null)
            {
                product.ImageUrl = await _imageService.SaveImageAsync(productDto.ImageFile, "products");
            }

            await _productRepository.CreateAsync(product);
            await _productRepository.SaveAsync();
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null) return;

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.CategoryId = productDto.CategoryId;
            product.Description = productDto.Description;
            if (productDto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl)) _imageService.DeleteImage(product.ImageUrl);
                product.ImageUrl = await _imageService.SaveImageAsync(productDto.ImageFile, "products");
            }

            _productRepository.Update(product);
            await _productRepository.SaveAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return;

            if (!string.IsNullOrEmpty(product.ImageUrl)) _imageService.DeleteImage(product.ImageUrl);
            await _productRepository.DeleteAsync(product.Id);
            await _productRepository.SaveAsync();
        }

        public async Task RestockProductAsync(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return;

            product.Stock += quantity;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();
        }

        public async Task<List<ProductDto>> GetOutOfStockProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products
                .Where(p => p.Stock == 0)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    ImageUrl = p.ImageUrl
                })
                .ToList();
        }
    }
}