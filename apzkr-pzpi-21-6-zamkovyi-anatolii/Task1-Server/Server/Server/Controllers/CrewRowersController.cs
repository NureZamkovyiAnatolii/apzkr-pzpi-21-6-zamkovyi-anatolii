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
    public class CrewRowersController : ControllerBase
    {
        private readonly ServerContext _context;

        public CrewRowersController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/CrewRowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrewRower>>> GetCrewRower()
        {
            if (_context.CrewRower == null)
            {
                return NotFound();
            }
            return await _context.CrewRower.ToListAsync();
        }

        // GET: api/CrewRowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CrewRower>> GetCrewRower(int id)
        {
            if (_context.CrewRower == null)
            {
                return NotFound();
            }

            var crewRower = await _context.CrewRower.FindAsync(id);

            if (crewRower == null)
            {
                return NotFound();
            }

            return crewRower;
        }

        // PUT: api/CrewRowers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrewRower(int id, CrewRower crewRower)
        {
            if (id != crewRower.CrewRowerId)
            {
                return BadRequest();
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.Athlete.AnyAsync(a => a.AthleteId == crewRower.AthleteId) ||
                !await _context.Crew.AnyAsync(c => c.CrewId == crewRower.CrewId))
            {
                return BadRequest("Invalid AthleteId or CrewId.");
            }

            _context.Entry(crewRower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrewRowerExists(crewRower.CrewRowerId))
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

        // POST: api/CrewRowers
        [HttpPost]
        public async Task<ActionResult<CrewRower>> PostCrewRower(CrewRower crewRower)
        {
            if (_context.CrewRower == null)
            {
                return Problem("Entity set 'ServerContext.CrewRower' is null.");
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.Athlete.AnyAsync(a => a.AthleteId == crewRower.AthleteId) ||
                !await _context.Crew.AnyAsync(c => c.CrewId == crewRower.CrewId))
            {
                return BadRequest("Invalid AthleteId or CrewId.");
            }

            _context.CrewRower.Add(crewRower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrewRower", new { id = crewRower.CrewRowerId }, crewRower);
        }

        // DELETE: api/CrewRowers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrewRower(int id)
        {
            if (_context.CrewRower == null)
            {
                return NotFound();
            }

            var crewRower = await _context.CrewRower.FindAsync(id);
            if (crewRower == null)
            {
                return NotFound();
            }

            _context.CrewRower.Remove(crewRower);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CrewRowerExists(int id)
        {
            return (_context.CrewRower?.Any(e => e.CrewRowerId == id)).GetValueOrDefault();
        }
    }
}
