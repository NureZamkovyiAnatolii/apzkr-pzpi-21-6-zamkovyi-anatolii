using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using WebApplication3.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private readonly ServerContext _context;

        public RacesController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Races
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Race>>> GetRaces()
        {
            if (_context.Race == null)
            {
                return NotFound("Entity set 'ServerContext.Race' is null.");
            }
            return await _context.Race.ToListAsync();
        }

        // GET: api/Races/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Race>> GetRace(int id)
        {
            if (_context.Race == null)
            {
                return NotFound();
            }

            var race = await _context.Race.FindAsync(id);

            if (race == null)
            {
                return NotFound();
            }

            return race;
        }

        // PUT: api/Races/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRace(int id, [FromBody] Race race)
        {
            if (id != race.RaceId)
            {
                return BadRequest();
            }

            // Перевірка на існування CompetitionId
            var competitionExists = await _context.Competition.AnyAsync(c => c.CompetitionId == race.CompetitionId);
            if (!competitionExists)
            {
                return BadRequest("Invalid CompetitionId.");
            }

            _context.Entry(race).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceExists(id))
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

        // POST: api/Races
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult<Race>> PostRace([FromBody] Race race)
        {
            if (_context.Race == null)
            {
                return Problem("Entity set 'ServerContext.Race' is null.");
            }

            // Перевірка на існування CompetitionId
            var competitionExists = await _context.Competition.AnyAsync(c => c.CompetitionId == race.CompetitionId);
            if (!competitionExists)
            {
                return BadRequest("Invalid CompetitionId.");
            }

            _context.Race.Add(race);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRace", new { id = race.RaceId }, race);
        }

        // DELETE: api/Races/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            if (_context.Race == null)
            {
                return NotFound("Entity set 'ServerContext.Race' is null.");
            }

            var race = await _context.Race.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }

            _context.Race.Remove(race);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RaceExists(int id)
        {
            return (_context.Race?.Any(e => e.RaceId == id)).GetValueOrDefault();
        }
    }
}
