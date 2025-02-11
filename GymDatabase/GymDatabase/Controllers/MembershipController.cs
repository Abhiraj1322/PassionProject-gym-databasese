using GymDatabase.Data;
using GymDatabase.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller for managing gym membership data, including CRUD operations (Create, Read, Update, Delete).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MembershipController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor that initializes the controller with the given database context.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public MembershipController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of all memberships with their related member training and payment data.
    /// </summary>
    /// <returns>A list of MembershipDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Membershipdto>>> GetMemberships()
    {
        var memberships = await _context.Memberships
            .Include(m => m.MemberTrainings)
            .Include(m => m.Memebrship_payments)
            .ToListAsync();

        var membershipDtos = memberships.Select(m => new Membershipdto
        {
            MembershipId = m.MembershipId,
            MemberName = m.MemberName,
            PlanType = m.PlanType,
            StartDate = m.StartDate,
            EndDate = m.EndDate,
        }).ToList();

        return Ok(membershipDtos);
    }

    /// <summary>
    /// Retrieves a specific membership by its ID.
    /// </summary>
    /// <param name="id">The ID of the membership to retrieve.</param>
    /// <returns>The membership data as a MembershipDto object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Membershipdto>> GetMembership(int id)
    {
        var membership = await _context.Memberships
            .Include(m => m.MemberTrainings)
            .Include(m => m.Memebrship_payments)
            .FirstOrDefaultAsync(m => m.MembershipId == id);

        if (membership == null)
        {
            return NotFound();
        }

        var membershipDto = new Membershipdto
        {
            MembershipId = membership.MembershipId,
            MemberName = membership.MemberName,
            PlanType = membership.PlanType,
            StartDate = membership.StartDate,
            EndDate = membership.EndDate,
        };

        return Ok(membershipDto);
    }

    /// <summary>
    /// Creates a new membership record.
    /// </summary>
    /// <param name="membership">The membership data to be added.</param>
    /// <returns>The newly created membership record.</returns>
    [HttpPost]
    public async Task<ActionResult<Membership>> PostMembership(Membership membership)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Memberships.Add(membership);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMembership), new { id = membership.MembershipId }, membership);
    }

    /// <summary>
    /// Updates an existing membership record.
    /// </summary>
    /// <param name="id">The ID of the membership to update.</param>
    /// <param name="membership">The updated membership data.</param>
    /// <returns>No content if successful, or a NotFound error if the membership does not exist.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMembership(int id, Membership membership)
    {
        if (id != membership.MembershipId)
        {
            return BadRequest("Membership ID mismatch.");
        }

        _context.Entry(membership).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MembershipExists(id))
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
    /// Deletes a membership record by its ID.
    /// </summary>
    /// <param name="id">The ID of the membership to delete.</param>
    /// <returns>No content if successful, or a NotFound error if the membership does not exist.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMembership(int id)
    {
        var membership = await _context.Memberships.FindAsync(id);
        if (membership == null)
        {
            return NotFound();
        }

        _context.Memberships.Remove(membership);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Checks if a membership exists by its ID.
    /// </summary>
    /// <param name="id">The ID of the membership to check.</param>
    /// <returns>True if the membership exists, false otherwise.</returns>
    private bool MembershipExists(int id)
    {
        return _context.Memberships.Any(m => m.MembershipId == id);
    }
}
