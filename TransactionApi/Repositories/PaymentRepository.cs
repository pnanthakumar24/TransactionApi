using Microsoft.EntityFrameworkCore;
using TransactionApi.Data;
using TransactionApi.Models;

namespace TransactionApi.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _db;
        public PaymentRepository(AppDbContext db) => _db = db;

        public async Task<Payment> CreateAsync(Payment payment)
        {
            // Ensure RequestId
            if (payment.RequestId == Guid.Empty)
                payment.RequestId = Guid.NewGuid();

            // Use UTC date for day grouping
            var today = DateTime.UtcNow.Date;
            var count = await _db.Payments.CountAsync(p => p.CreatedAt >= today && p.CreatedAt < today.AddDays(1));
            var sequence = count + 1;
            payment.Reference = $"PAY-{today:yyyyMMdd}-{sequence:0000}";

            if (payment.CreatedAt == default) payment.CreatedAt = DateTime.UtcNow;

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _db.Payments.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Payment?> UpdateAsync(Guid requestId, Payment payment)
        {
            var existing = await _db.Payments.FirstOrDefaultAsync(p => p.RequestId == requestId);
            if (existing == null) return null;

            // Only allow updating amount and currency for now
            existing.Amount = payment.Amount;
            existing.Currency = payment.Currency;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid requestId)
        {
            var existing = await _db.Payments.FirstOrDefaultAsync(p => p.RequestId == requestId);
            if (existing == null) return false;
            _db.Payments.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
