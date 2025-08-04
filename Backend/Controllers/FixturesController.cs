using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FixturesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FixturesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fixture>>> GetFixtures()
        {
            return await _context.Fixtures.Include(f => f.Game).Include(f => f.Registrations).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Fixture>> CreateFixture(Fixture fixture, [FromQuery] int adminEmployeeId)
        {
            var admin = await _context.Employees.FindAsync(adminEmployeeId);
            if (admin == null || !admin.IsAdmin)
                return BadRequest("Only admin employees can create fixtures.");
            // Set NumberOfGames for Chess semi/final
            if (fixture.Game?.Name == "Chess" && fixture.IsKnockout && fixture.NumberOfGames < 3)
            {
                // Logic to determine if this is semi-final or final (to be expanded)
                fixture.NumberOfGames = 3;
            }
            _context.Fixtures.Add(fixture);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFixtures), new { id = fixture.FixtureId }, fixture);
        }

        [HttpPost("generate-knockout")]
        public async Task<IActionResult> GenerateKnockoutFixtures([FromBody] List<int> registrationIds, [FromQuery] int gameId, [FromQuery] GameFormat format, [FromQuery] int adminEmployeeId)
        {
            var admin = await _context.Employees.FindAsync(adminEmployeeId);
            if (admin == null || !admin.IsAdmin)
                return BadRequest("Only admin employees can generate fixtures.");

            var registrations = await _context.Registrations.Where(r => registrationIds.Contains(r.RegistrationId)).ToListAsync();
            if (registrations.Count < 2)
                return BadRequest("At least two teams/players required.");

            // Calculate next power of 2
            int totalTeams = registrations.Count;
            int nextPowerOf2 = 1;
            while (nextPowerOf2 < totalTeams) nextPowerOf2 *= 2;
            int byes = nextPowerOf2 - totalTeams;

            // Shuffle registrations for random matchups
            var rng = new Random();
            registrations = registrations.OrderBy(x => rng.Next()).ToList();

            // Assign byes
            var allTeams = new List<Registration>(registrations);
            for (int i = 0; i < byes; i++)
            {
                var byeFixture = new Fixture
                {
                    GameId = gameId,
                    Format = format,
                    ScheduledTime = DateTime.Now,
                    Registrations = new List<Registration> { registrations[i] },
                    IsBye = true,
                    IsKnockout = true
                };
                _context.Fixtures.Add(byeFixture);
            }

            // Generate rounds
            int round = 1;
            var currentRoundTeams = allTeams;
            var fixturesCreated = new List<Fixture>();
            while (currentRoundTeams.Count > 1)
            {
                var nextRoundTeams = new List<Registration>();
                for (int i = 0; i < currentRoundTeams.Count; i += 2)
                {
                    var fixture = new Fixture
                    {
                        GameId = gameId,
                        Format = format,
                        ScheduledTime = DateTime.Now.AddDays(round),
                        Registrations = new List<Registration>(),
                        IsKnockout = true,
                        NumberOfGames = (round >= 3 && format == GameFormat.MensSingles && _context.Games.Find(gameId)?.Name == "Chess") ? 3 : 1
                    };
                    fixture.Registrations.Add(currentRoundTeams[i]);
                    if (i + 1 < currentRoundTeams.Count)
                        fixture.Registrations.Add(currentRoundTeams[i + 1]);
                    else
                        fixture.IsBye = true;
                    _context.Fixtures.Add(fixture);
                    fixturesCreated.Add(fixture);
                }
                // Prepare next round teams (winners placeholder)
                currentRoundTeams = new List<Registration>();
                for (int i = 0; i < fixturesCreated.Count; i++)
                {
                    // Placeholder: add winner registrationId after match is played
                    currentRoundTeams.Add(new Registration { RegistrationId = 0 });
                }
                round++;
                fixturesCreated.Clear();
            }

            await _context.SaveChangesAsync();
            return Ok("Knockout fixtures generated up to final, with byes and placeholders.");
        }
    }
}
