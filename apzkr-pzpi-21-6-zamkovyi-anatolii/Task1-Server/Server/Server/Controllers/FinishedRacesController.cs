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
    public class FinishedRacesController : ControllerBase
    {
        private readonly ServerContext _context;

        public FinishedRacesController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/FinishedRaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinishedRace>>> GetFinishedRace()
        {
            if (_context.FinishedRace == null)
            {
                return NotFound();
            }
            return await _context.FinishedRace.ToListAsync();
        }

        // GET: api/FinishedRaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinishedRace>> GetFinishedRace(int id)
        {
            if (_context.FinishedRace == null)
            {
                return NotFound();
            }

            var finishedRace = await _context.FinishedRace.FindAsync(id);

            if (finishedRace == null)
            {
                return NotFound();
            }

            return finishedRace;
        }

        // PUT: api/FinishedRaces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinishedRace(int id, FinishedRace finishedRace)
        {
            if (id != finishedRace.FinishedRaceId)
            {
                return BadRequest();
            }

            // Перевірка наявності зовнішніх сутностей (якщо є зв'язки)
            if (!await _context.Race.AnyAsync(r => r.RaceId == finishedRace.RaceId))
            {
                return BadRequest("Invalid RaceId.");
            }

            _context.Entry(finishedRace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinishedRaceExists(finishedRace.FinishedRaceId))
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

        // POST: api/FinishedRaces
        [HttpPost]
        public async Task<ActionResult<FinishedRace>> PostFinishedRace(FinishedRace finishedRace)
        {
            if (_context.FinishedRace == null)
            {
                return Problem("Entity set 'ServerContext.FinishedRace' is null.");
            }

            // Перевірка наявності зовнішніх сутностей (якщо є зв'язки)
            if (!await _context.Race.AnyAsync(r => r.RaceId == finishedRace.RaceId))
            {
                return BadRequest("Invalid RaceId.");
            }

            _context.FinishedRace.Add(finishedRace);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinishedRace", new { id = finishedRace.FinishedRaceId }, finishedRace);
        }

        // DELETE: api/FinishedRaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinishedRace(int id)
        {
            if (_context.FinishedRace == null)
            {
                return NotFound();
            }

            var finishedRace = await _context.FinishedRace.FindAsync(id);
            if (finishedRace == null)
            {
                return NotFound();
            }

            _context.FinishedRace.Remove(finishedRace);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinishedRaceExists(int id)
        {
            return (_context.FinishedRace?.Any(e => e.FinishedRaceId == id)).GetValueOrDefault();
        }
    }
}
