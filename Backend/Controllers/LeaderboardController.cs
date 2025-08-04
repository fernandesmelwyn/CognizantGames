using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LeaderboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardEntry>>> GetLeaderboard()
        {
            return await _context.LeaderboardEntries.Include(l => l.Employee).Include(l => l.Game).ToListAsync();
        }
    }
}
