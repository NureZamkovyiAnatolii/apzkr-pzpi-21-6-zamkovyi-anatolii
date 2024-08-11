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
    public class ResultHandicapsController : ControllerBase
    {
        private readonly ServerContext _context;

        public ResultHandicapsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/ResultHandicaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultHandicap>>> GetResultHandicap()
        {
            if (_context.ResultHandicap == null)
            {
                return NotFound();
            }
            return await _context.ResultHandicap.ToListAsync();
        }

        // GET: api/ResultHandicaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultHandicap>> GetResultHandicap(int id)
        {
            if (_context.ResultHandicap == null)
            {
                return NotFound();
            }

            var resultHandicap = await _context.ResultHandicap.FindAsync(id);

            if (resultHandicap == null)
            {
                return NotFound();
            }

            return resultHandicap;
        }

        // PUT: api/ResultHandicaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResultHandicap(int id, ResultHandicap resultHandicap)
        {
            if (id != resultHandicap.ResultHandicapId)
            {
                return BadRequest();
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.CrewHandicap.AnyAsync(ch => ch.CrewHandicapId == resultHandicap.CrewHandicapId))
            {
                return BadRequest("Invalid CrewHandicapId.");
            }

            _context.Entry(resultHandicap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultHandicapExists(resultHandicap.ResultHandicapId))
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

        // POST: api/ResultHandicaps
        [HttpPost]
        public async Task<ActionResult<ResultHandicap>> PostResultHandicap(ResultHandicap resultHandicap)
        {
            if (_context.ResultHandicap == null)
            {
                return Problem("Entity set 'ServerContext.ResultHandicap' is null.");
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.CrewHandicap.AnyAsync(ch => ch.CrewHandicapId == resultHandicap.CrewHandicapId))
            {
                return BadRequest("Invalid CrewHandicapId.");
            }

            _context.ResultHandicap.Add(resultHandicap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResultHandicap", new { id = resultHandicap.ResultHandicapId }, resultHandicap);
        }

        // DELETE: api/ResultHandicaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResultHandicap(int id)
        {
            if (_context.ResultHandicap == null)
            {
                return NotFound();
            }

            var resultHandicap = await _context.ResultHandicap.FindAsync(id);
            if (resultHandicap == null)
            {
                return NotFound();
            }

            _context.ResultHandicap.Remove(resultHandicap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultHandicapExists(int id)
        {
            return (_context.ResultHandicap?.Any(e => e.ResultHandicapId == id)).GetValueOrDefault();
        }
    }
}
