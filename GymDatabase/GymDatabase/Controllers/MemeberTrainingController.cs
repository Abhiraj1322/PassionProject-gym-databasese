using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller for managing gym member training data, including CRUD operations (Create, Read, Update, Delete).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MemberTrainingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor that initializes the controller with the given database context.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public MemberTrainingController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of all member trainings with related membership and trainee data.
    /// </summary>
    /// <returns>A list of MemberTrainingDto objects.</returns>
    [HttpGet("List")]
    public async Task<ActionResult<IEnumerable<MemberTrainingdto>>> GetMemberTrainings()
    {
        var memberTrainings = await _context.MemberTrainings
            .Include(mt => mt.Membership)  // Include related membership data
            .Include(mt => mt.Trainee)     // Include related trainee data
            .ToListAsync();

        var memberTrainingDtos = memberTrainings.Select(mt => new MemberTrainingdto
        {
            MemberTrainingId = mt.MemberTrainingId,
            MembershipId = mt.MembershipId,
            TrainerId = mt.TrainerId,
            TrainingType = mt.TrainingType,
            Membership = new Membership
            {
                MembershipId = mt.Membership.MembershipId,
            },
            Trainee = new Trainee
            {
                TrainerId = mt.Trainee.TrainerId,
            }
        }).ToList();

        return Ok(memberTrainingDtos);
    }

    /// <summary>
    /// Creates a new member training record.
    /// </summary>
    /// <param name="memberTraining">The member training data to be added.</param>
    /// <returns>The newly created member training record.</returns>
    [HttpPost]
    public async Task<ActionResult<MemberTraining>> PostMemberTraining(MemberTraining memberTraining)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.MemberTrainings.Add(memberTraining);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMemberTrainings), new { id = memberTraining.MemberTrainingId }, memberTraining);
    }

    /// <summary>
    /// Updates an existing member training record.
    /// </summary>
    /// <param name="id">The ID of the member training to update.</param>
    /// <param name="memberTraining">The updated member training data.</param>
    /// <returns>No content if successful, or a NotFound error if the member training does not exist.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMemberTraining(int id, MemberTraining memberTraining)
    {
        if (id != memberTraining.MemberTrainingId)
        {
            return BadRequest("MemberTraining ID mismatch.");
        }

        _context.Entry(memberTraining).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.MemberTrainings.Any(mt => mt.MemberTrainingId == id))
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
    /// Deletes a member training record by its ID.
    /// </summary>
    /// <param name="id">The ID of the member training to delete.</param>
    /// <returns>No content if successful, or a NotFound error if the member training does not exist.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMemberTraining(int id)
    {
        var memberTraining = await _context.MemberTrainings.FindAsync(id);
        if (memberTraining == null)
        {
            return NotFound();
        }

        _context.MemberTrainings.Remove(memberTraining);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
