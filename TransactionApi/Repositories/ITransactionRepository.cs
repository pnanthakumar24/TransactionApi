using TransactionApi.Models;

namespace TransactionApi.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByUserAsync(int userId);
    }
}