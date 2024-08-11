using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using BC = BCrypt.Net.BCrypt;

namespace WebApplication3.Controllers
{
    public class AthletesController : Controller
    {
        private readonly WebApplication3Context _context;

        public AthletesController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: Athletes
        public async Task<IActionResult> Index()
        {
              return _context.Athlete != null ? 
                          View(await _context.Athlete.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.Athlete'  is null.");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Athlete>>> GetAthletes()
        {
            if (_context.Athlete == null)
            {
                return NotFound("Entity set 'WebApplication3Context.Athlete' is null.");
            }

            var athletes = await _context.Athlete.ToListAsync();
            return Ok(athletes);
        }

        // GET: Athletes/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(athlete);
        }

        // GET: Athletes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Athletes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AthleteId,AthleteName,Password,BirthDate,PhoneNumber,CoachId")] Athlete athlete)
        {
            if (ModelState.IsValid)
            {
                // Перевірка наявності тренера в базі даних за ID
                var coachExists = _context.Coach.Any(c => c.CoachId == athlete.CoachId);

                if (!coachExists)
                {
                    ModelState.AddModelError("CoachId", "Такого тренера не існує.");
                    return View(athlete);
                }
                athlete.Password = BC.HashPassword(athlete.Password);
                _context.Add(athlete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(athlete);
        }

        // GET: Athletes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Athlete == null)
            {
                return NotFound();
            }

            var athlete = await _context.Athlete.FindAsync(id);
            if (athlete == null)
            {
                return NotFound();
            }
            return View(athlete);
        }

        // POST: Athletes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AthleteId,AthleteName,Password,BirthDate,PhoneNumber,CoachId")] Athlete athlete)
        {
            if (id != athlete.AthleteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            return View(athlete);
        }

        // GET: Athletes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(athlete);
        }

        // POST: Athletes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Athlete == null)
            {
                return Problem("Entity set 'WebApplication3Context.Athlete'  is null.");
            }
            var athlete = await _context.Athlete.FindAsync(id);
            if (athlete != null)
            {
                _context.Athlete.Remove(athlete);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AthleteExists(int id)
        {
          return (_context.Athlete?.Any(e => e.AthleteId == id)).GetValueOrDefault();
        }
    }
}
