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
    public class ResultsController : ControllerBase
    {
        private readonly ServerContext _context;

        public ResultsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResult()
        {
            if (_context.Result == null)
            {
                return NotFound();
            }
            return await _context.Result.ToListAsync();
        }

        // GET: api/Results/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            if (_context.Result == null)
            {
                return NotFound();
            }

            var result = await _context.Result.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Results/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(int id, Result result)
        {
            if (id != result.ResultId)
            {
                return BadRequest();
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.Crew.AnyAsync(c => c.CrewId == result.CrewId))
            {
                return BadRequest("Invalid CrewId.");
            }

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(result.ResultId))
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

        // POST: api/Results
        [HttpPost]
        public async Task<ActionResult<Result>> PostResult(Result result)
        {
            if (_context.Result == null)
            {
                return Problem("Entity set 'ServerContext.Result' is null.");
            }

            // Перевірка наявності зовнішніх сутностей
            if (!await _context.Crew.AnyAsync(c => c.CrewId == result.CrewId))
            {
                return BadRequest("Invalid CrewId.");
            }

            _context.Result.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResult", new { id = result.ResultId }, result);
        }

        // DELETE: api/Results/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            if (_context.Result == null)
            {
                return NotFound();
            }

            var result = await _context.Result.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            _context.Result.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultExists(int id)
        {
            return (_context.Result?.Any(e => e.ResultId == id)).GetValueOrDefault();
        }
    }
}
