using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

public class MembershipController : Controller
{
    private readonly ApplicationDbContext _context;

    public MembershipController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Membership (Index Page)
    public async Task<IActionResult> Index()
    {
        var memberships = await _context.Memberships
            .Select(m => new Membershipdto
            {
                MembershipId = m.MembershipId,
                MemberName = m.MemberName,
                PlanType = m.PlanType,
                StartDate = m.StartDate,
                EndDate = m.EndDate
            })
            .ToListAsync();

        return View(memberships); // Now passing List<Membershipdto>
    }


    // GET: Membership/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var membership = await _context.Memberships
            .Include(m => m.MemberTrainings)
            .Include(m => m.Memebrship_payments)
            .FirstOrDefaultAsync(m => m.MembershipId == id);

        if (membership == null) return NotFound();

        // Convert Membership to Membershipdto
        var dto = new Membershipdto
        {
            MembershipId = membership.MembershipId,
            MemberName = membership.MemberName,
            PlanType = membership.PlanType,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate
        };

        return View(dto); // Now passing Membershipdto
    }

    // GET: Membership/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Membership/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Membershipdto dto)
    {
        if (ModelState.IsValid)
        {
            var membership = new Membership
            {
                MemberName = dto.MemberName,
                PlanType = dto.PlanType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Add(membership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(dto); // Ensure the view expects Membershipdto
    }

    // GET: Membership/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var membership = await _context.Memberships.FindAsync(id);
        if (membership == null) return NotFound();

        // Convert Membership to Membershipdto
        var dto = new Membershipdto
        {
            MembershipId = membership.MembershipId,
            MemberName = membership.MemberName,
            PlanType = membership.PlanType,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate
        };

        return View(dto); // Now passing Membershipdto
    }

    // POST: Membership/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Membershipdto dto)
    {
        if (id != dto.MembershipId) return NotFound();

        if (ModelState.IsValid)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null) return NotFound();

            // Update only necessary fields
            membership.MemberName = dto.MemberName;
            membership.PlanType = dto.PlanType;
            membership.StartDate = dto.StartDate;
            membership.EndDate = dto.EndDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MembershipExists(id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(dto); // Ensure the view expects Membershipdto
    }


    // GET: Membership/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.MembershipId == id);

        if (membership == null) return NotFound();

        // Convert Membership to Membershipdto
        var dto = new Membershipdto
        {
            MembershipId = membership.MembershipId,
            MemberName = membership.MemberName,
            PlanType = membership.PlanType,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate
        };

        return View(dto); // Now passing Membershipdto
    }

    // POST: Membership/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var membership = await _context.Memberships.FindAsync(id);
        if (membership != null)
        {
            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }




    private async Task<bool> MembershipExists(int id)
    {
        return await _context.Memberships.AnyAsync(m => m.MembershipId == id);
    }
}