using ECommerceApp.DTOs;
using ECommerceApp.Helpers;
using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductFilterDto filter);
        IEnumerable<Product> GetSortedProducts(IEnumerable<Product> products,string sortBy, bool descending);
        IEnumerable<Product> Search(IEnumerable<Product> products, string productName);
        Task<IEnumerable<Product>> GetAllProducts();
        IEnumerable<Product> GetPaginatedProducts(IEnumerable<Product> products, int pageNumber, int pageSize);
        Task<Product> GetProductWithCategoryAndReviewsByIdAsync(int id);
        Task<IEnumerable<ProductSalesDto>> GetTopSellingProducts(int count);
        Task<PaginatedList<ProductDto>> GetProductsAsync(int pageNumber, int pageSize);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDto productDto);
        Task UpdateProductAsync(ProductDto productDto);
        Task DeleteProductAsync(int id);
        Task RestockProductAsync(int id, int quantity);
        Task<List<ProductDto>> GetOutOfStockProductsAsync();
    }
}
