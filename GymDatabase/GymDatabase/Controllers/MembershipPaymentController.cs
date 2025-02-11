using GymDatabase.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GymDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipPaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembershipPaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all membership payments.
        /// </summary>
        /// <returns>A list of membership payments.</returns>
        /// <response code="200">Returns the list of membership payments.</response>
        /// <response code="404">If no payments are found.</response>
        [HttpGet("ListMembershipPayments")]
        public async Task<ActionResult<IEnumerable<Memebrship_paymentdto>>> ListMembershipPayments()
        {
            var membershipPayments = await _context.Memberships_payment.ToListAsync();

            var paymentDtos = new List<Memebrship_paymentdto>();

            foreach (var payment in membershipPayments)
            {
                // Fetch the related membership using MembershipId
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m => m.MembershipId == payment.MembershipId);

                // Map to DTO
                var paymentDto = new Memebrship_paymentdto
                {
                    PaymentId = payment.PaymentId,
                    MembershipId = payment.MembershipId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate
                };

                // Optionally, you can add additional membership details to the DTO, if needed.
                // For example:
                if (membership != null)
                {
                    // Add some fields from Membership entity if necessary
                    // paymentDto.MembershipName = membership.Name;  // Example: Add membership name
                }

                paymentDtos.Add(paymentDto);
            }

            return Ok(paymentDtos);
        }
    

        /// <summary>
        /// Retrieves a specific membership payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the membership payment to retrieve.</param>
        /// <returns>The details of the requested membership payment.</returns>
        /// <response code="200">Returns the membership payment details.</response>
        /// <response code="404">If the membership payment with the specified ID is not found.</response>
        [Route("FindMembershipPayment/{id}")]
        [HttpGet]
        public async Task<ActionResult<Memebrship_paymentdto>> FindMembershipPayment(int id)
        {
            var payment = await _context.Memberships_payment
                .FirstOrDefaultAsync(mp => mp.PaymentId == id);

            if (payment == null)
            {
                return NotFound("Membership payment not found.");
            }

            var paymentDto = new Memebrship_paymentdto
            {
                PaymentId = payment.PaymentId,
                MembershipId = payment.MembershipId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate
            };

            return Ok(paymentDto);
        }

        /// <summary>
        /// Adds a new membership payment.
        /// </summary>
        /// <param name="paymentDto">The details of the membership payment to add.</param>
        /// <returns>The newly created membership payment with its ID.</returns>
        /// <response code="201">Returns the created membership payment details.</response>
        /// <response code="400">If the provided membership payment data is invalid.</response>
        [Route("AddMembershipPayment")]
        [HttpPost]
        public async Task<ActionResult<Memebrship_paymentdto>> AddMembershipPayment(Memebrship_paymentdto paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest("Invalid payment data.");
            }

            var paymentEntity = new Memebrship_payment
            {
                MembershipId = paymentDto.MembershipId,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate
            };

            _context.Memberships_payment.Add(paymentEntity);
            await _context.SaveChangesAsync();

            paymentDto.PaymentId = paymentEntity.PaymentId;

            return CreatedAtAction(nameof(FindMembershipPayment), new { id = paymentEntity.PaymentId }, paymentDto);
        }

        /// <summary>
        /// Updates an existing membership payment.
        /// </summary>
        /// <param name="id">The ID of the membership payment to update.</param>
        /// <param name="paymentDto">The updated membership payment details.</param>
        /// <returns>The updated membership payment details.</returns>
        /// <response code="200">Returns the updated membership payment details.</response>
        /// <response code="400">If the provided membership payment data is invalid.</response>
        /// <response code="404">If the membership payment with the specified ID is not found.</response>
        [Route("UpdateMembershipPayment/{id}")]
        [HttpPut]
        public async Task<ActionResult<Memebrship_paymentdto>> UpdateMembershipPayment(int id, Memebrship_paymentdto paymentDto)
        {
            if (id != paymentDto.PaymentId)
            {
                return BadRequest("Payment ID mismatch.");
            }

            var payment = await _context.Memberships_payment.FindAsync(id);

            if (payment == null)
            {
                return NotFound("Membership payment not found.");
            }

            payment.MembershipId = paymentDto.MembershipId;
            payment.Amount = paymentDto.Amount;
            payment.PaymentDate = paymentDto.PaymentDate;

            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(paymentDto);
        }

        /// <summary>
        /// Deletes a membership payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the membership payment to delete.</param>
        /// <returns>A confirmation of the deletion.</returns>
        /// <response code="204">If the membership payment was deleted successfully.</response>
        /// <response code="404">If the membership payment with the specified ID is not found.</response>
        [Route("DeleteMembershipPayment/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMembershipPayment(int id)
        {
            var payment = await _context.Memberships_payment.FindAsync(id);

            if (payment == null)
            {
                return NotFound("Membership payment not found.");
            }

            _context.Memberships_payment.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
