using ECommerceApp.Helpers;
using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PaginatedList<Transaction>> GetTransactionsAsync(int pageNumber, int pageSize)
        {
            var transactions = _transactionRepository.GetAllTransactions();
            var query = transactions
                .OrderByDescending(t => t.Date)
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Status = t.Status,
                    Date = t.Date,
                    User = new User
                    {
                        UserName = t.User.UserName ?? "Deleted User"
                    }
                });

            return await PaginatedList<Transaction>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
