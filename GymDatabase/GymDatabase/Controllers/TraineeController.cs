using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

public class TraineeController : Controller
{
    private readonly ApplicationDbContext _context;

    public TraineeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Trainee (Index Page)
    public async Task<IActionResult> Index()
    {
        var trainees = await _context.Trainees
            .Select(t => new TraineeDTO
            {
                TrainerId = t.TrainerId,
                Name = t.Name,
                Specialty = t.Specialty
            })
            .ToListAsync();

        return View(trainees); // Now passing List<TraineeDTO>
    }

    // GET: Trainee/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var trainee = await _context.Trainees
            .FirstOrDefaultAsync(t => t.TrainerId == id);

        if (trainee == null) return NotFound();

        // Convert Trainee to TraineeDTO
        var dto = new TraineeDTO
        {
            TrainerId = trainee.TrainerId,
            Name = trainee.Name,
            Specialty = trainee.Specialty
        };

        return View(dto); // Now passing TraineeDTO
    }

    // GET: Trainee/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Trainee/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TraineeDTO dto)
    {
        if (ModelState.IsValid)
        {
            var trainee = new Trainee
            {
                Name = dto.Name,
                Specialty = dto.Specialty
            };

            _context.Add(trainee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(dto); // Ensure the view expects TraineeDTO
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var membership = await _context.Memberships.FindAsync(id);
        if (membership == null) return NotFound();

        var dto = new Membershipdto
        {
            MembershipId = membership.MembershipId,
            MemberName = membership.MemberName,
            PlanType = membership.PlanType,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate
        };

        return View(dto);
    }

    // POST: Membership/Edit/5
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Membershipdto dto)
    {
        if (id != dto.MembershipId) return BadRequest("ID mismatch");

        if (ModelState.IsValid)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null) return NotFound();

            // Update fields
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
        return View(dto);
    }

    private async Task<bool> MembershipExists(int id)
    {
        return await _context.Memberships.AnyAsync(e => e.MembershipId == id);
    }


// GET: Trainee/Delete/5
public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var trainee = await _context.Trainees
            .FirstOrDefaultAsync(t => t.TrainerId == id);

        if (trainee == null) return NotFound();

        // Convert Trainee to TraineeDTO
        var dto = new TraineeDTO
        {
            TrainerId = trainee.TrainerId,
            Name = trainee.Name,
            Specialty = trainee.Specialty
        };

        return View(dto); // Now passing TraineeDTO
    }

    // POST: Trainee/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var trainee = await _context.Trainees.FindAsync(id);
        if (trainee != null)
        {
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> TraineeExists(int id)
    {
        return await _context.Trainees.AnyAsync(t => t.TrainerId == id);
    }
}
