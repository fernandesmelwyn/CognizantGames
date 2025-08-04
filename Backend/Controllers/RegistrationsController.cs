using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RegistrationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations()
        {
            return await _context.Registrations.Include(r => r.Employee).Include(r => r.PartnerEmployee).Include(r => r.Game).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Registration>> Register(Registration registration)
        {
            var employee = await _context.Employees.FindAsync(registration.EmployeeId);
            if (employee == null)
                return BadRequest("Employee not found.");
            // Prevent duplicate registration for same game and format
            bool exists = await _context.Registrations.AnyAsync(r =>
                r.EmployeeId == registration.EmployeeId &&
                r.GameId == registration.GameId &&
                r.Format == registration.Format);
            if (exists)
                return BadRequest("Already registered for this game and format.");
            // Prevent duplicate team nomination in same format
            if (registration.PartnerEmployeeId.HasValue)
            {
                bool teamExists = await _context.Registrations.AnyAsync(r =>
                    r.GameId == registration.GameId &&
                    r.Format == registration.Format &&
                    (r.EmployeeId == registration.EmployeeId || r.PartnerEmployeeId == registration.EmployeeId || r.EmployeeId == registration.PartnerEmployeeId || r.PartnerEmployeeId == registration.PartnerEmployeeId));
                if (teamExists)
                    return BadRequest("Employee or partner already registered in another team for this format.");
            }
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRegistrations), new { id = registration.RegistrationId }, registration);
        }
    }
}
