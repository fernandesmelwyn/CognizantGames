using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("upcoming-fixtures")]
        public async Task<ActionResult<IEnumerable<object>>> GetUpcomingFixtures()
        {
            var now = DateTime.Now;
            var fixtures = await _context.Fixtures
                .Include(f => f.Game)
                .Include(f => f.Registrations)
                .Where(f => f.ScheduledTime > now && !f.IsCompleted)
                .Select(f => new {
                    f.FixtureId,
                    f.ScheduledTime,
                    Game = new { f.Game.GameId, f.Game.Name },
                    Registrations = f.Registrations.Select(r => new { r.RegistrationId, r.EmployeeId, r.PartnerEmployeeId })
                })
                .ToListAsync();
            return Ok(fixtures);
        }

        [HttpGet("leaderboard")]
        public async Task<ActionResult<IEnumerable<LeaderboardEntry>>> GetLeaderboard()
        {
            return await _context.LeaderboardEntries.Include(l => l.Employee).Include(l => l.Game).ToListAsync();
        }
    }
}
