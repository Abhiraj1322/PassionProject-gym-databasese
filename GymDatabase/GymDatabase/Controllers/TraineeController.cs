using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller for managing trainee data, including CRUD operations (Create, Read, Update, Delete).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TraineesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor that initializes the controller with the given database context.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public TraineesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of all trainees with their basic details.
    /// </summary>
    /// <returns>A list of TraineeDTO objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TraineeDTO>>> GetTrainees()
    {
        var trainees = await _context.Trainees
            .Select(t => new TraineeDTO
            {
                TrainerId = t.TrainerId,
                Name = t.Name,
                Specialty = t.Specialty
            })
            .ToListAsync();

        return Ok(trainees);
    }

    /// <summary>
    /// Retrieves a specific trainee by their ID.
    /// </summary>
    /// <param name="id">The ID of the trainee to retrieve.</param>
    /// <returns>The trainee's details or a 404 error if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeDTO>> GetTrainee(int id)
    {
        var trainee = await _context.Trainees
            .Where(t => t.TrainerId == id)
            .Select(t => new TraineeDTO
            {
                TrainerId = t.TrainerId,
                Name = t.Name,
                Specialty = t.Specialty
            })
            .FirstOrDefaultAsync();

        if (trainee == null)
        {
            return NotFound();
        }

        return Ok(trainee);
    }

    /// <summary>
    /// Updates a specific trainee's data.
    /// </summary>
    /// <param name="id">The ID of the trainee to update.</param>
    /// <param name="traineeDTO">The updated trainee data.</param>
    /// <returns>No content if successful, or a NotFound error if the trainee does not exist.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrainee(int id, TraineeDTO traineeDTO)
    {
        if (id != traineeDTO.TrainerId)
        {
            return BadRequest();
        }

        var trainee = await _context.Trainees.FindAsync(id);
        if (trainee == null)
        {
            return NotFound();
        }

        trainee.Name = traineeDTO.Name;
        trainee.Specialty = traineeDTO.Specialty;

        _context.Entry(trainee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TraineeExists(id))
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

    /// <summary>
    /// Creates a new trainee record.
    /// </summary>
    /// <param name="traineeDTO">The trainee data to be added.</param>
    /// <returns>The newly created trainee record.</returns>
    [HttpPost]
    public async Task<ActionResult<Trainee>> PostTrainee(TraineeDTO traineeDTO)
    {
        var trainee = new Trainee
        {
            Name = traineeDTO.Name,
            Specialty = traineeDTO.Specialty
        };

        _context.Trainees.Add(trainee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTrainee), new { id = trainee.TrainerId }, trainee);
    }

    /// <summary>
    /// Deletes a specific trainee by their ID.
    /// </summary>
    /// <param name="id">The ID of the trainee to delete.</param>
    /// <returns>No content if successful, or a NotFound error if the trainee does not exist.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        var trainee = await _context.Trainees.FindAsync(id);
        if (trainee == null)
        {
            return NotFound();
        }

        _context.Trainees.Remove(trainee);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TraineeExists(int id)
    {
        return _context.Trainees.Any(e => e.TrainerId == id);
    }
}
