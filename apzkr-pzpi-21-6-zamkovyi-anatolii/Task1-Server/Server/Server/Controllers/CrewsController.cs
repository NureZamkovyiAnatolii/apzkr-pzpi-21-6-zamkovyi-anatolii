using System.Collections.Generic;
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
    public class CrewsController : ControllerBase
    {
        private readonly ServerContext _context;

        public CrewsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Crews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Crew>>> GetCrews()
        {
            if (_context.Crew == null)
            {
                return NotFound();
            }
            return await _context.Crew.ToListAsync();
        }

        // GET: api/Crews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Crew>> GetCrew(int id)
        {
            if (_context.Crew == null)
            {
                return NotFound();
            }
            var crew = await _context.Crew.FindAsync(id);

            if (crew == null)
            {
                return NotFound();
            }

            return crew;
        }

        // PUT: api/Crews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrew(int id, [Bind("CrewId,BoatType,CompetitionId,RaceId,StartNumber")] Crew crew)
        {
            if (id != crew.CrewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Перевірка на існування CompetitionId
                    var competitionExists = await _context.Competition.AnyAsync(c => c.CompetitionId == crew.CompetitionId);
                    if (!competitionExists)
                    {
                        ModelState.AddModelError("CompetitionId", "Invalid CompetitionId.");
                    }

                    // Перевірка на існування RaceId
                    var raceExists = await _context.Race.AnyAsync(r => r.RaceId == crew.RaceId);
                    if (!raceExists)
                    {
                        ModelState.AddModelError("RaceId", "Invalid RaceId.");
                    }

                    if (ModelState.IsValid)
                    {
                        _context.Update(crew);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewExists(crew.CrewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }
        [HttpPost("assign-random-start-numbers{raceId}")]
        public async Task<ActionResult> AssignRandomStartNumbers(int raceId)
        {
            try
            {
                var random = new Random();

                // Отримуємо всі Crew з заданим RaceId
                var crews = await _context.Crew
                    .Where(ch => ch.RaceId == raceId)
                    .ToListAsync();

                if (crews == null || !crews.Any())
                {
                    return NotFound("No Crew found for the specified RaceId.");
                }

                foreach (var crew in crews)
                {
                    // Встановлюємо випадковий стартовий номер
                    crew.StartNumber = random.Next(1, 1000); // або будь-який інший діапазон, що вам потрібен
                }

                await _context.SaveChangesAsync();

                return Ok("Start numbers assigned successfully.");
            }
            catch (Exception ex)
            {
                // Логування помилки
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // POST: api/Crews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Crew>> PostCrew([Bind("CrewId,BoatType,CompetitionId,RaceId,StartNumber")] Crew crew)
        {
            // Перевірка на існування CompetitionId
            var competitionExists = await _context.Competition.AnyAsync(c => c.CompetitionId == crew.CompetitionId);
            if (!competitionExists)
            {
                ModelState.AddModelError("CompetitionId", "Invalid CompetitionId.");
            }

            // Перевірка на існування RaceId
            var raceExists = await _context.Race.AnyAsync(r => r.RaceId == crew.RaceId);
            if (!raceExists)
            {
                ModelState.AddModelError("RaceId", "Invalid RaceId.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(crew);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrew", new { id = crew.CrewId }, crew);
        }


        // DELETE: api/Crews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrew(int id)
        {
            if (_context.Crew == null)
            {
                return NotFound();
            }
            var crew = await _context.Crew.FindAsync(id);
            if (crew == null)
            {
                return NotFound();
            }

            _context.Crew.Remove(crew);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CrewExists(int id)
        {
            return (_context.Crew?.Any(e => e.CrewId == id)).GetValueOrDefault();
        }
    }
}
