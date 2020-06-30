using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentsApi.Models;

namespace PaymentsApi.Controllers
{
    [Route("payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentContext _context;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(PaymentContext context, ILogger<PaymentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments()
        {
            return await _context.Payments.Select(x => ItemToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(long id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(long id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Payment>> DeletePayment(long id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        private bool PaymentExists(long id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        private static PaymentDTO ItemToDTO(Payment payment) =>
      new PaymentDTO
      {
          Id = payment.Id,
          Name = payment.Name,
          IsPayed = payment.IsPayed,
          AmountToPay = payment.AmountToPay
      };
    }
}
