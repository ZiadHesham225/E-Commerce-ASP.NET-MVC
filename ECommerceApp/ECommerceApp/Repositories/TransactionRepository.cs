using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await CreateAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByOrderAsync(int orderId)
        {
            return await dbSet.Where(t => t.OrderId == orderId).ToListAsync();
        }
        public IQueryable<Transaction> GetAllTransactions()
        {
            return _context.Transactions
                .Include(t => t.User);
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(string userId)
        {
            return await dbSet.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}