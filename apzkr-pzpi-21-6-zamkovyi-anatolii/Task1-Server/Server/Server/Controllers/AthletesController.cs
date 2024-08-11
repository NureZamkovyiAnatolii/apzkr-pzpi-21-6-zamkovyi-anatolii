using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using WebApplication3.Models;
using BC = BCrypt.Net.BCrypt;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AthletesController : ControllerBase
    {
        private readonly ServerContext _context;

        public AthletesController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Athletes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Athlete>>> GetAthletes()
        {
            return _context.Athlete != null ?
                      await _context.Athlete.ToListAsync() :
                      Problem("Entity set 'WebApplication3Context.Athlete' is null.");
        }
        // GET: api/Athletes/Me
        [HttpGet("Me")]
        public async Task<ActionResult<Coach>> GetAuthenticatedAthlete()
        {
            var athleteId = HttpContext.Session.GetInt32("AthleteId");
            if (athleteId == null)
            {
                return Unauthorized();
            }

            var athlete = await _context.Coach.FirstOrDefaultAsync(c => c.CoachId == athleteId);

            if (athlete == null)
            {
                return NotFound();
            }

            return athlete;
        }
        // GET: api/Athletes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Athlete>> GetAthlete(int? id)
        {
            if (id == null || _context.Athlete == null)
            {
                return NotFound();
            }

            var athlete = await _context.Athlete
                .FirstOrDefaultAsync(m => m.AthleteId == id);
            if (athlete == null)
            {
                return NotFound();
            }

            return athlete;
        }
        public class CompetitionWithCrewsDto
        {
            public int? CompetitionId { get; set; }
            public int? OrganizationId { get; set; }
            public int? CoachId { get; set; }
            public string CompetitionName { get; set; }
            public string CompetitionCountry { get; set; }
            public string CompetitionCity { get; set; }
            public DateTime RegistrationDeadline { get; set; }
            public DateTime CompetitionDate { get; set; }
            public List<CrewDto> Crews { get; set; }
        }

        public class CrewDto
        {
            public int CrewId { get; set; }
            public BoatType BoatType { get; set; }
            public int CompetitionId { get; set; }
            public int RaceId { get; set; }
            public int StartNumber { get; set; }
            public List<AthleteDto> Athletes { get; set; } // Список спортсменів
            public TimeSpan? TimeTaken { get; set; } // Додано час на змаганні (опціонально)
        }


        public class AthleteDto
        {
            public int AthleteId { get; set; }
            public string AthleteName { get; set; }
            // Додайте інші властивості спортсмена за потреби
        }
        public class CompetitionsResponseDto
        {
            public List<CompetitionWithCrewsDto> FutureCompetitions { get; set; }
            public List<CompetitionWithCrewsDto> Results { get; set; }
        }


        [HttpGet("GetCompetitionsByAthletes")]
        public async Task<ActionResult<CompetitionsResponseDto>> GetCompetitionsByAthletes(int? athleteId)
        {
            if (athleteId == null)
            {
                return NotFound();
            }

            // Знайти всі екіпажі, в яких є цей спортсмен
            var crewIds = await _context.CrewRower
                .Where(cr => cr.AthleteId == athleteId)
                .Select(cr => cr.CrewId)
                .ToListAsync();

            // Знайти всі змагання, де є ці екіпажі
            var allCompetitions = await _context.Competition
                .Where(c => _context.Crew.Any(crew => crew.CompetitionId == c.CompetitionId && crewIds.Contains(crew.CrewId)))
                .ToListAsync();

            // Розділити змагання на ті, що мають результати, і ті, що їх не мають
            var futureCompetitions = allCompetitions
    .Select(c => new CompetitionWithCrewsDto
    {
        CompetitionId = c.CompetitionId,
        OrganizationId = c.OrganizationId,
        CoachId = c.CoachId,
        CompetitionName = c.CompetitionName,
        CompetitionCountry = c.CompetitionCountry,
        CompetitionCity = c.CompetitionCity,
        RegistrationDeadline = c.RegistrationDeadline,
        CompetitionDate = c.CompetitioneDate,
        Crews = _context.Crew
            .Where(crew => crew.CompetitionId == c.CompetitionId &&
                           !_context.FinishedRace.Any(fr => fr.RaceId == crew.RaceId))
            .Select(crew => new CrewDto
            {
                CrewId = crew.CrewId,
                BoatType = crew.BoatType,
                CompetitionId = crew.CompetitionId,
                RaceId = crew.RaceId,
                StartNumber = crew.StartNumber,
                Athletes = _context.CrewRower
                    .Where(cr => cr.CrewId == crew.CrewId)
                    .Select(cr => new AthleteDto
                    {
                        AthleteId = cr.AthleteId,
                        AthleteName = _context.Athlete
                            .Where(a => a.AthleteId == cr.AthleteId)
                            .Select(a => a.AthleteName)
                            .FirstOrDefault()
                    })
                    .ToList(),
                TimeTaken = null // Немає результату, тому `TimeTaken` залишаємо як `null`
            })
            .ToList()
    })
    .Where(c => _context.Result.Any(r => _context.Crew.Any(cr => cr.CrewId == r.CrewId && cr.CompetitionId == c.CompetitionId)))
    .ToList();


            var results = allCompetitions
    .Select(c => new CompetitionWithCrewsDto
    {
        CompetitionId = c.CompetitionId,
        OrganizationId = c.OrganizationId,
        CoachId = c.CoachId,
        CompetitionName = c.CompetitionName,
        CompetitionCountry = c.CompetitionCountry,
        CompetitionCity = c.CompetitionCity,
        RegistrationDeadline = c.RegistrationDeadline,
        CompetitionDate = c.CompetitioneDate,
        Crews = _context.Crew
            .Where(crew => crew.CompetitionId == c.CompetitionId &&
                           _context.FinishedRace.Any(fr => fr.RaceId == crew.RaceId))
            .Select(crew => new CrewDto
            {
                CrewId = crew.CrewId,
                BoatType = crew.BoatType,
                CompetitionId = crew.CompetitionId,
                RaceId = crew.RaceId,
                StartNumber = crew.StartNumber,
                Athletes = _context.CrewRower
                    .Where(cr => cr.CrewId == crew.CrewId)
                    .Select(cr => new AthleteDto
                    {
                        AthleteId = cr.AthleteId,
                        AthleteName = _context.Athlete
                            .Where(a => a.AthleteId == cr.AthleteId)
                            .Select(a => a.AthleteName)
                            .FirstOrDefault()
                    })
                    .ToList(),
                TimeTaken = _context.Result
                                .Where(r => r.CrewId == crew.CrewId)
                                .Select(r => r.TimeTaken)
                                .FirstOrDefault() // Отримати перший результат (якщо існує)
            })
            .ToList()
    })
    .Where(c => _context.Result.Any(r => _context.Crew.Any(cr => cr.CrewId == r.CrewId && cr.CompetitionId == c.CompetitionId)))
    .ToList();

            var response = new CompetitionsResponseDto
            {
                FutureCompetitions = futureCompetitions,
                Results = results
            };

            return Ok(response);
        }


        // POST: api/Athletes
        [HttpPost]
        public async Task<ActionResult<Athlete>> CreateAthlete([Bind("AthleteId,AthleteName,Password,BirthDate,PhoneNumber,CoachId")] Athlete athlete)
        {
            if (ModelState.IsValid)
            {
                // Check if the coach referenced by CoachId exists in the database
                var coachExists = await _context.Coach.AnyAsync(c => c.CoachId == athlete.CoachId);
                if (!coachExists)
                {
                    ModelState.AddModelError("CoachId", "Такого тренера не існує.");
                    return BadRequest(ModelState);
                }

                // Hash the athlete's password before saving to the database
                athlete.Password = BC.HashPassword(athlete.Password);

                // Add athlete to the context and save changes
                _context.Add(athlete);
                await _context.SaveChangesAsync();

                // Return 201 Created status with the newly created athlete
                return CreatedAtAction(nameof(GetAthlete), new { id = athlete.AthleteId }, athlete);
            }

            // If ModelState is not valid, return bad request with ModelState errors
            return BadRequest(ModelState);
        }

        // PUT: api/Athletes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAthlete(int id, Athlete athlete)
        {
            if (id != athlete.AthleteId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    athlete.Password = BC.HashPassword(athlete.Password);
                    _context.Update(athlete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AthleteExists(athlete.AthleteId))
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

        // DELETE: api/Athletes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAthlete(int id)
        {
            if (_context.Athlete == null)
            {
                return Problem("Entity set 'WebApplication3Context.Athlete' is null.");
            }

            var athlete = await _context.Athlete.FindAsync(id);
            if (athlete == null)
            {
                return NotFound();
            }

            _context.Athlete.Remove(athlete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AthleteExists(int id)
        {
            return _context.Athlete.Any(e => e.AthleteId == id);
        }
    }
}
