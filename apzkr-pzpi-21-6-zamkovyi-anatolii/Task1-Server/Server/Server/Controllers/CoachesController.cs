using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using WebApplication3.Models;
using BC = BCrypt.Net.BCrypt;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachesController : ControllerBase
    {
        private readonly ServerContext _context;

        public CoachesController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Coaches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coach>>> GetCoaches()
        {
            return _context.Coach != null ?
                      await _context.Coach.ToListAsync() :
                      Problem("Entity set 'ServerContext.Coach'  is null.");
        }

        // GET: api/Coaches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coach>> GetCoach(int? id)
        {
            if (id == null || _context.Coach == null)
            {
                return NotFound();
            }

            var coach = await _context.Coach
                .FirstOrDefaultAsync(m => m.CoachId == id);
            if (coach == null)
            {
                return NotFound();
            }

            return coach;
        }
        // GET: api/Coaches/Me
        [HttpGet("Me")]
        public async Task<ActionResult<Coach>> GetAuthenticatedCoach()
        {
            var coachId = HttpContext.Session.GetInt32("CoachId");
            if (coachId == null)
            {
                return Unauthorized();
            }

            var coach = await _context.Coach.FirstOrDefaultAsync(c => c.CoachId == coachId);

            if (coach == null)
            {
                return NotFound();
            }

            return coach;
        }
        // POST: api/Coaches
        [HttpPost]
        public async Task<ActionResult<Coach>> CreateCoach([Bind("CoachId,CoachName,Password,BirthDate,PhoneNumber,Country")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                coach.Password = BC.HashPassword(coach.Password);
                _context.Add(coach);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCoach), new { id = coach.CoachId }, coach);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Coaches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCoach(int id, [Bind("CoachId,CoachName,Password,BirthDate,PhoneNumber,Country")] Coach coach)
        {
            if (id != coach.CoachId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    coach.Password = BC.HashPassword(coach.Password);
                    _context.Update(coach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coach.CoachId))
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
            return BadRequest(ModelState);
        }
        // GET: api/Coaches/MyAthletes
        [HttpGet("GetAthletesByCoach")]
        public async Task<ActionResult<IEnumerable<Athlete>>> GetAthletesByCoach(int? coachId)
        {
            if (coachId == null)
            {
                return NotFound();
            }

            var athletes = await _context.Athlete.Where(a => a.CoachId == coachId).ToListAsync();
            return athletes;
        }
        // DELETE: api/Coaches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            if (_context.Coach == null)
            {
                return Problem("Entity set 'ServerContext.Coach'  is null.");
            }

            var coach = await _context.Coach.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }

            _context.Coach.Remove(coach);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoachExists(int id)
        {
            return (_context.Coach?.Any(e => e.CoachId == id)).GetValueOrDefault();
        }
    }
}
