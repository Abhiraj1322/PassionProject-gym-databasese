using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using GymDatabase.Data.Migrations;

namespace GymDatabase.Controllers
{
    public class MemeberTrainingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemeberTrainingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MemberTraining (Index Page)
        public async Task<IActionResult> Index()
        {
            var trainings = await _context.MemberTrainings
                .Select(t => new MemberTrainingdto
                {
                    MemberTrainingId = t.MemberTrainingId,
                    MembershipId = t.MembershipId,
                    TrainerId = t.TrainerId,
                    TrainingType = t.TrainingType
                })
                .ToListAsync();

            return View(trainings);
        }

        // GET: MemberTraining/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var training = await _context.MemberTrainings
                .Include(t => t.Membership)
                .Include(t => t.Trainee)
                .FirstOrDefaultAsync(t => t.MemberTrainingId == id);

            if (training == null) return NotFound();

            var dto = new MemberTrainingdto
            {
                MemberTrainingId = training.MemberTrainingId,
                MembershipId = training.MembershipId,
                TrainerId = training.TrainerId,
                TrainingType = training.TrainingType
            };

            return View(dto);
        }

        // GET: MemberTraining/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberTraining/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberTrainingdto dto)
        {
            if (ModelState.IsValid)
            {
                var training = new MemeberTraining
                {
                    MemberTrainingId = dto.MemberTrainingId,  // Correctly map MembershipId
                    TrainerId = dto.TrainerId,        // Correctly map TrainerId
                    TrainingType = dto.TrainingType   // Correctly map TrainingType
                };

                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: MemberTraining/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var training = await _context.MemberTrainings.FindAsync(id);
            if (training == null) return NotFound();

            var dto = new MemberTrainingdto
            {
                MemberTrainingId = training.MemberTrainingId,
                MembershipId = training.MembershipId,
                TrainerId = training.TrainerId,
                TrainingType = training.TrainingType
            };

            return View(dto);
        }

        // POST: MemberTraining/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberTrainingdto dto)
        {
            if (id != dto.MemberTrainingId) return NotFound();

            if (ModelState.IsValid)
            {
                var training = await _context.MemberTrainings.FindAsync(id);
                if (training == null) return NotFound();

                training.MembershipId = dto.MembershipId;
                training.TrainerId = dto.TrainerId;
                training.TrainingType = dto.TrainingType;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TrainingExists(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: MemberTraining/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var training = await _context.MemberTrainings
                .FirstOrDefaultAsync(t => t.MemberTrainingId == id);

            if (training == null) return NotFound();

            var dto = new MemberTrainingdto
            {
                MemberTrainingId = training.MemberTrainingId,
                MembershipId = training.MembershipId,
                TrainerId = training.TrainerId,
                TrainingType = training.TrainingType
            };

            return View(dto);
        }

        // POST: MemberTraining/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.MemberTrainings.FindAsync(id);
            if (training != null)
            {
                _context.MemberTrainings.Remove(training);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TrainingExists(int id)
        {
            return await _context.MemberTrainings.AnyAsync(t => t.MemberTrainingId == id);
        }
    }
}
