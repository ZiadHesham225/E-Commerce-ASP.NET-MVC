using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();

        Task<Transaction> GetTransactionByIdAsync(int id);

        Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(string userId);

        Task<IEnumerable<Transaction>> GetTransactionsByOrderAsync(int orderId);

        Task AddTransactionAsync(Transaction transaction);
        IQueryable<Transaction> GetAllTransactions();
    }
}