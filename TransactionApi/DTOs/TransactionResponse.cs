using System;

namespace TransactionApi.DTOs
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public string CardMasked { get; set; } = null!;
        public string CardHolder { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}