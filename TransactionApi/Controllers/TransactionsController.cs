using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransactionApi.DTOs;
using TransactionApi.Models;
using TransactionApi.Repositories;

namespace TransactionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactions;
        public TransactionsController(ITransactionRepository transactions) => _transactions = transactions;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreateRequest req)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (req.Amount <= 0) return BadRequest("Amount must be greater than zero");

            int userId = int.Parse(userIdClaim.Value);

            // Mask card: keep last 4 digits
            var last4 = req.CardNumber?.Trim().Replace(" ", "") ?? "";
            if (last4.Length < 4) return BadRequest("Invalid card number");
            last4 = last4.Substring(last4.Length - 4);
            var masked = $"****-****-****-{last4}";

            var txn = new Transaction
            {
                UserId = userId,
                CardMasked = masked,
                CardHolder = req.CardHolder,
                Amount = req.Amount,
                Currency = req.Currency,
                Status = "Approved", // demo: assume approved
                CreatedAt = DateTime.UtcNow
            };

            var created = await _transactions.CreateAsync(txn);

            var resp = new TransactionResponse
            {
                Id = created.Id,
                CardMasked = created.CardMasked,
                CardHolder = created.CardHolder,
                Amount = created.Amount,
                Currency = created.Currency,
                Status = created.Status,
                CreatedAt = created.CreatedAt
            };

            return Ok(resp);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMine()
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var list = await _transactions.GetByUserAsync(userId);
            var resp = list.Select(t => new TransactionResponse
            {
                Id = t.Id,
                CardMasked = t.CardMasked,
                CardHolder = t.CardHolder,
                Amount = t.Amount,
                Currency = t.Currency,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            });

            return Ok(resp);
        }
    }
}