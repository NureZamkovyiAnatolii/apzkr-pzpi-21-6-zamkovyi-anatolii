using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using WebApplication3.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewHandicapsController : ControllerBase
    {
        private readonly ServerContext _context;

        public CrewHandicapsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/CrewHandicaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrewHandicap>>> GetCrewHandicap()
        {
            if (_context.CrewHandicap == null)
            {
                return NotFound();
            }
            return await _context.CrewHandicap.ToListAsync();
        }

        // GET: api/CrewHandicaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CrewHandicap>> GetCrewHandicap(int id)
        {
            if (_context.CrewHandicap == null)
            {
                return NotFound();
            }

            var crewHandicap = await _context.CrewHandicap.FindAsync(id);

            if (crewHandicap == null)
            {
                return NotFound();
            }

            return crewHandicap;
        }

        // PUT: api/CrewHandicaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrewHandicap(int id, CrewHandicap crewHandicap)
        {
            if (id != crewHandicap.CrewHandicapId)
            {
                return BadRequest();
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.Competition.AnyAsync(c => c.CompetitionId == crewHandicap.CompetitionId))
            {
                return BadRequest("Invalid CompetitionId.");
            }

            if (!await _context.RaceWithHandicap.AnyAsync(r => r.RaceWithHandicapId == crewHandicap.RaceWithHandicapId))
            {
                return BadRequest("Invalid RaceWithHandicapId.");
            }

            _context.Entry(crewHandicap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrewHandicapExists(crewHandicap.CrewHandicapId))
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

        // POST: api/CrewHandicaps
        [HttpPost]
        public async Task<ActionResult<CrewHandicap>> PostCrewHandicap(CrewHandicap crewHandicap)
        {
            if (_context.CrewHandicap == null)
            {
                return Problem("Entity set 'ServerContext.CrewHandicap'  is null.");
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.Competition.AnyAsync(c => c.CompetitionId == crewHandicap.CompetitionId))
            {
                return BadRequest("Invalid CompetitionId.");
            }

            if (!await _context.RaceWithHandicap.AnyAsync(r => r.RaceWithHandicapId == crewHandicap.RaceWithHandicapId))
            {
                return BadRequest("Invalid RaceWithHandicapId.");
            }

            _context.CrewHandicap.Add(crewHandicap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrewHandicap", new { id = crewHandicap.CrewHandicapId }, crewHandicap);
        }

        // DELETE: api/CrewHandicaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrewHandicap(int id)
        {
            if (_context.CrewHandicap == null)
            {
                return NotFound();
            }

            var crewHandicap = await _context.CrewHandicap.FindAsync(id);
            if (crewHandicap == null)
            {
                return NotFound();
            }

            _context.CrewHandicap.Remove(crewHandicap);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("assign-random-start-numbers/{raceWithHandicapId}")]
        public async Task<ActionResult> AssignRandomStartNumbers(int raceWithHandicapId)
        {
            try
            {
                var random = new Random();

                // Отримуємо всі CrewHandicap з заданим RaceWithHandicapId
                var crewHandicaps = await _context.CrewHandicap
                    .Where(ch => ch.RaceWithHandicapId == raceWithHandicapId)
                    .ToListAsync();

                if (crewHandicaps == null || !crewHandicaps.Any())
                {
                    Console.WriteLine("No CrewHandicaps found for the specified RaceWithHandicapId.");
                    return NotFound("No CrewHandicaps found for the specified RaceWithHandicapId.");
                }
                
                foreach (var crewHandicap in crewHandicaps)
                {
                    // Встановлюємо випадковий стартовий номер
                    crewHandicap.StartNumber = random.Next(1, 1000); // або будь-який інший діапазон, що вам потрібен
                }

                await _context.SaveChangesAsync();
                Console.WriteLine("Start numbers assigned successfully.");
                return Ok("Start numbers assigned successfully.");
            }
            catch (Exception ex)
            {
                // Логування помилки
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool CrewHandicapExists(int id)
        {
            return (_context.CrewHandicap?.Any(e => e.CrewHandicapId == id)).GetValueOrDefault();
        }
    }
}
