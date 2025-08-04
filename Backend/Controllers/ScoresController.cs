using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ScoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Score>>> GetScores()
        {
            return await _context.Scores.Include(s => s.Fixture).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Score>> AddScore(Score score, [FromQuery] int adminEmployeeId)
        {
            var admin = await _context.Employees.FindAsync(adminEmployeeId);
            if (admin == null || !admin.IsAdmin)
                return BadRequest("Only admin employees can update scores.");
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetScores), new { id = score.ScoreId }, score);
        }
    }
}
