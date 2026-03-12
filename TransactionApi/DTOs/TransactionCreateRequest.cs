namespace TransactionApi.DTOs
{
    public class TransactionCreateRequest
    {
        public string CardNumber { get; set; } = null!; // raw input from caller (we will mask)
        public string CardHolder { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }
}