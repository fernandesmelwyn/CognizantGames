using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetGames()
        {
            var games = await _context.Games
                .Include(g => g.SupportedFormats)
                .Select(g => new {
                    g.GameId,
                    g.Name,
                    SupportedFormats = g.SupportedFormats != null ? g.SupportedFormats.Select(f => new { f.GameFormatTypeId, f.Name }).Cast<object>().ToList() : new List<object>()
                })
                .ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetGame(int id)
        {
            var game = await _context.Games
                .Include(g => g.SupportedFormats)
                .Select(g => new {
                    g.GameId,
                    g.Name,
                    SupportedFormats = g.SupportedFormats != null ? g.SupportedFormats.Select(f => new { f.GameFormatTypeId, f.Name }).Cast<object>().ToList() : new List<object>()
                })
                .FirstOrDefaultAsync(g => g.GameId == id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
        }

        [HttpPut("{id}/formats")]
        public async Task<IActionResult> UpdateGameFormats(int id, [FromBody] List<int> formatIds)
        {
            var game = await _context.Games.Include(g => g.SupportedFormats).FirstOrDefaultAsync(g => g.GameId == id);
            if (game == null) return NotFound();
            var formats = await _context.GameFormatTypes.Where(f => formatIds.Contains(f.GameFormatTypeId)).ToListAsync();
            game.SupportedFormats = formats;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
