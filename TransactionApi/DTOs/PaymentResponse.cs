using System;

namespace TransactionApi.DTOs
{
    public class PaymentResponse
    {
        public string Reference { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime CreatedAt { get; set; }
    }
}
