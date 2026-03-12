using TransactionApi.Models;

namespace TransactionApi.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> UpdateAsync(Guid requestId, Payment payment);
        Task<bool> DeleteAsync(Guid requestId);
    }
}
