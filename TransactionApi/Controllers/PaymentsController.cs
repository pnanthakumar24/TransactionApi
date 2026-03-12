using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionApi.DTOs;
using TransactionApi.Models;
using TransactionApi.Repositories;

namespace TransactionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _payments;
        public PaymentsController(IPaymentRepository payments) => _payments = payments;

        [HttpPost]
        public async Task<IActionResult> Create(PaymentCreateRequest req)
        {
            if (req.Amount <= 0) return BadRequest("Amount must be greater than zero");

            var payment = new Payment
            {
                RequestId = req.RequestId ?? Guid.NewGuid(),
                Amount = req.Amount,
                Currency = req.Currency,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _payments.CreateAsync(payment);

            var resp = new PaymentResponse
            {
                Reference = created.Reference,
                Amount = created.Amount,
                Currency = created.Currency,
                CreatedAt = created.CreatedAt
            };

            return Ok(resp);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _payments.GetAllAsync();
            var resp = list.Select(p => new PaymentResponse
            {
                Reference = p.Reference,
                Amount = p.Amount,
                Currency = p.Currency,
                CreatedAt = p.CreatedAt
            });

            return Ok(resp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PaymentUpdateRequest req)
        {
            if (req.Amount <= 0) return BadRequest("Amount must be greater than zero");

            var updateModel = new Payment
            {
                Amount = req.Amount,
                Currency = req.Currency
            };

            var updated = await _payments.UpdateAsync(id, updateModel);
            if (updated == null) return NotFound();

            var resp = new PaymentResponse
            {
                Reference = updated.Reference,
                Amount = updated.Amount,
                Currency = updated.Currency,
                CreatedAt = updated.CreatedAt
            };

            return Ok(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _payments.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
