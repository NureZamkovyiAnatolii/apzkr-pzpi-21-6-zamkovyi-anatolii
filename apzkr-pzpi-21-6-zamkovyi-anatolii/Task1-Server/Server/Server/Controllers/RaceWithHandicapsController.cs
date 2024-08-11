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
    public class RaceWithHandicapsController : ControllerBase
    {
        private readonly ServerContext _context;

        public RaceWithHandicapsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/RaceWithHandicaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceWithHandicap>>> GetRaceWithHandicaps()
        {
            if (_context.RaceWithHandicap == null)
            {
                return NotFound("Entity set 'ServerContext.RaceWithHandicap' is null.");
            }
            return await _context.RaceWithHandicap.ToListAsync();
        }

        // GET: api/RaceWithHandicaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RaceWithHandicap>> GetRaceWithHandicap(int id)
        {
            if (_context.RaceWithHandicap == null)
            {
                return NotFound();
            }

            var raceWithHandicap = await _context.RaceWithHandicap.FindAsync(id);

            if (raceWithHandicap == null)
            {
                return NotFound();
            }

            return raceWithHandicap;
        }

        // PUT: api/RaceWithHandicaps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRaceWithHandicap(int id, [FromBody] RaceWithHandicap raceWithHandicap)
        {
            if (id != raceWithHandicap.RaceWithHandicapId)
            {
                return BadRequest();
            }

            // Перевірка на існування CompetitionId
            var competitionExists = await _context.Competition.AnyAsync(c => c.CompetitionId == raceWithHandicap.CompetitionId);
            if (!competitionExists)
            {
                return BadRequest("Invalid CompetitionId.");
            }

            _context.Entry(raceWithHandicap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceWithHandicapExists(id))
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

        // POST: api/RaceWithHandicaps
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult<RaceWithHandicap>> PostRaceWithHandicap([FromBody] RaceWithHandicap raceWithHandicap)
        {
            if (_context.RaceWithHandicap == null)
            {
                return Problem("Entity set 'ServerContext.RaceWithHandicap' is null.");
            }

            // Перевірка на існування CompetitionId
            var competitionExists = await _context.Competition.AnyAsync(c => c.CompetitionId == raceWithHandicap.CompetitionId);
            if (!competitionExists)
            {
                return BadRequest("Invalid CompetitionId.");
            }

            _context.RaceWithHandicap.Add(raceWithHandicap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRaceWithHandicap", new { id = raceWithHandicap.RaceWithHandicapId }, raceWithHandicap);
        }

        // DELETE: api/RaceWithHandicaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaceWithHandicap(int id)
        {
            if (_context.RaceWithHandicap == null)
            {
                return NotFound("Entity set 'ServerContext.RaceWithHandicap' is null.");
            }

            var raceWithHandicap = await _context.RaceWithHandicap.FindAsync(id);
            if (raceWithHandicap == null)
            {
                return NotFound();
            }

            _context.RaceWithHandicap.Remove(raceWithHandicap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RaceWithHandicapExists(int id)
        {
            return (_context.RaceWithHandicap?.Any(e => e.RaceWithHandicapId == id)).GetValueOrDefault();
        }
    }
}
