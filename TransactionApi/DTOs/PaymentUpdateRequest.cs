using System;

namespace TransactionApi.DTOs
{
    public class PaymentUpdateRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }
}
