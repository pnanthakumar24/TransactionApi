using System;

namespace TransactionApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        // Store only masked card or last4 for compliance demos
        public string CardMasked { get; set; } = null!;
        public string CardHolder { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}