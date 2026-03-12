using System;

namespace TransactionApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public Guid RequestId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Reference { get; set; } = null!; // PAY-YYYYMMDD-####
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
