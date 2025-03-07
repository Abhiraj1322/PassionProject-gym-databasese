using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymDatabase.Controllers
{
    public class MembershipPaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipPaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipPayment
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Memberships_payment
                .Select(p => new Memebrship_paymentdto
                {
                    PaymentId = p.PaymentId,
                    MembershipId = p.MembershipId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();

            return View(payments);
        }

        // GET: MembershipPayment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _context.Memberships_payment
                .Where(p => p.PaymentId == id)
                .Select(p => new Memebrship_paymentdto
                {
                    PaymentId = p.PaymentId,
                    MembershipId = p.MembershipId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate
                })
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: MembershipPayment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipPayment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId, MembershipId, Amount, PaymentDate")] Memebrship_paymentdto paymentDto)
        {
            if (ModelState.IsValid)
            {
                var payment = new Memebrship_payment
                {
                    MembershipId = paymentDto.MembershipId,
                    Amount = paymentDto.Amount,
                    PaymentDate = paymentDto.PaymentDate
                };

                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentDto);
        }

    

        // POST: MembershipPayment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId, MembershipId, Amount, PaymentDate")] Memebrship_paymentdto paymentDto)
        {
            if (id != paymentDto.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var payment = await _context.Memberships_payment.FindAsync(id);

                    if (payment == null)
                    {
                        return NotFound();
                    }

                    payment.MembershipId = paymentDto.MembershipId;
                    payment.Amount = paymentDto.Amount;
                    payment.PaymentDate = paymentDto.PaymentDate;

                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipPaymentExists(paymentDto.PaymentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(paymentDto);
        }

        // GET: MembershipPayment/Delete/5 (Shows the delete confirmation page)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Memberships_payment
                .Where(m => m.PaymentId == id)
                .Select(p => new Memebrship_paymentdto
                {
                    PaymentId = p.PaymentId,
                    MembershipId = p.MembershipId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate
                })
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Memberships_payment.FindAsync(id);

            if (payment != null)
            {
                _context.Memberships_payment.Remove(payment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }




        private bool MembershipPaymentExists(int id)
        {
            return _context.Memberships_payment.Any(e => e.PaymentId == id);
        }
    }
}
