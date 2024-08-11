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
    public class FinishedRaceWithHandicapsController : ControllerBase
    {
        private readonly ServerContext _context;

        public FinishedRaceWithHandicapsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/FinishedRaceWithHandicaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinishedRaceWithHandicap>>> GetFinishedRaceWithHandicap()
        {
            if (_context.FinishedRaceWithHandicap == null)
            {
                return NotFound();
            }
            return await _context.FinishedRaceWithHandicap.ToListAsync();
        }

        // GET: api/FinishedRaceWithHandicaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinishedRaceWithHandicap>> GetFinishedRaceWithHandicap(int id)
        {
            if (_context.FinishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            var finishedRaceWithHandicap = await _context.FinishedRaceWithHandicap.FindAsync(id);

            if (finishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            return finishedRaceWithHandicap;
        }

        // PUT: api/FinishedRaceWithHandicaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinishedRaceWithHandicap(int id, FinishedRaceWithHandicap finishedRaceWithHandicap)
        {
            if (id != finishedRaceWithHandicap.FinishedRaceWithHandicapId)
            {
                return BadRequest();
            }

            // Перевірка наявності зовнішніх сутностей (якщо є зв'язки)
            if (!await _context.RaceWithHandicap.AnyAsync(r => r.RaceWithHandicapId == finishedRaceWithHandicap.RaceWithHandicapId))
            {
                return BadRequest("Invalid RaceWithHandicapId.");
            }

            _context.Entry(finishedRaceWithHandicap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinishedRaceWithHandicapExists(finishedRaceWithHandicap.FinishedRaceWithHandicapId))
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

        // POST: api/FinishedRaceWithHandicaps
        [HttpPost]
        public async Task<ActionResult<FinishedRaceWithHandicap>> PostFinishedRaceWithHandicap(FinishedRaceWithHandicap finishedRaceWithHandicap)
        {
            if (_context.FinishedRaceWithHandicap == null)
            {
                return Problem("Entity set 'ServerContext.FinishedRaceWithHandicap' is null.");
            }

            // Перевірка наявності зовнішніх сутностей (якщо є зв'язки)
            if (!await _context.RaceWithHandicap.AnyAsync(r => r.RaceWithHandicapId == finishedRaceWithHandicap.RaceWithHandicapId))
            {
                return BadRequest("Invalid RaceWithHandicapId.");
            }

            _context.FinishedRaceWithHandicap.Add(finishedRaceWithHandicap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinishedRaceWithHandicap", new { id = finishedRaceWithHandicap.FinishedRaceWithHandicapId }, finishedRaceWithHandicap);
        }

        // DELETE: api/FinishedRaceWithHandicaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinishedRaceWithHandicap(int id)
        {
            if (_context.FinishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            var finishedRaceWithHandicap = await _context.FinishedRaceWithHandicap.FindAsync(id);
            if (finishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            _context.FinishedRaceWithHandicap.Remove(finishedRaceWithHandicap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinishedRaceWithHandicapExists(int id)
        {
            return (_context.FinishedRaceWithHandicap?.Any(e => e.FinishedRaceWithHandicapId == id)).GetValueOrDefault();
        }
    }
}
