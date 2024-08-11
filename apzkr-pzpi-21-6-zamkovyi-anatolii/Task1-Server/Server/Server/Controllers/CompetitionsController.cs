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
    public class CompetitionsController : ControllerBase
    {
        private readonly ServerContext _context;

        public CompetitionsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Competitions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
            if (_context.Competition == null)
            {
                return NotFound();
            }
            return await _context.Competition.ToListAsync();
        }

        // GET: api/Competitions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Competition>> GetCompetition(int id)
        {
            if (_context.Competition == null)
            {
                return NotFound();
            }

            var competition = await _context.Competition.FindAsync(id);

            if (competition == null)
            {
                return NotFound();
            }

            return competition;
        }

        // PUT: api/Competitions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetition(int id, Competition competition)
        {
            if (id != competition.CompetitionId)
            {
                return BadRequest();
            }

            // Перевірка на наявність організації та тренера
            var organizationExists = await _context.Organization.AnyAsync(o => o.OrganizationId == competition.OrganizationId);
            var coachExists = await _context.Coach.AnyAsync(c => c.CoachId == competition.CoachId);

            if (!organizationExists && !coachExists)
            {
                return NotFound($"Neither Organization with ID {competition.OrganizationId} nor Coach with ID {competition.CoachId} found.");
            }

            _context.Entry(competition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitionExists(id))
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

        // POST: api/Competitions
        [HttpPost]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
            if (_context.Competition == null)
            {
                return Problem("Entity set 'ServerContext.Competition' is null.");
            }

            // Перевірка на наявність організації та тренера
            var organizationExists = await _context.Organization.AnyAsync(o => o.OrganizationId == competition.OrganizationId);
            var coachExists = await _context.Coach.AnyAsync(c => c.CoachId == competition.CoachId);

            if (!organizationExists && !coachExists)
            {
                return NotFound($"Neither Organization with ID {competition.OrganizationId} nor Coach with ID {competition.CoachId} found.");
            }

            _context.Competition.Add(competition);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompetition), new { id = competition.CompetitionId }, competition);
        }


        // DELETE: api/Competitions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            if (_context.Competition == null)
            {
                return NotFound();
            }

            var competition = await _context.Competition.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }

            _context.Competition.Remove(competition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompetitionExists(int id)
        {
            return (_context.Competition?.Any(e => e.CompetitionId == id)).GetValueOrDefault();
        }
    }
}
