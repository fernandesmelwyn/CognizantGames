using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameFormatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GameFormatsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetGameFormats()
        {
            var formats = await _context.GameFormatTypes
                .Select(f => new {
                    f.GameFormatTypeId,
                    f.Name,
                    Games = f.Games != null ? f.Games.Select(g => new { g.GameId, g.Name }).Cast<object>().ToList() : new List<object>()
                })
                .ToListAsync();
            return Ok(formats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameFormatType>> GetGameFormat(int id)
        {
            var format = await _context.GameFormatTypes.Include(f => f.Games).FirstOrDefaultAsync(f => f.GameFormatTypeId == id);
            if (format == null) return NotFound();
            return format;
        }

        [HttpPost]
        public async Task<ActionResult<GameFormatType>> CreateGameFormat(GameFormatType format)
        {
            _context.GameFormatTypes.Add(format);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGameFormat), new { id = format.GameFormatTypeId }, format);
        }
    }
}
