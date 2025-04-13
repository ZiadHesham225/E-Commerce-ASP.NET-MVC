using ECommerceApp.Helpers;
using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface ITransactionService
    {
        Task<PaginatedList<Transaction>> GetTransactionsAsync(int pageNumber, int pageSize);
    }
}
