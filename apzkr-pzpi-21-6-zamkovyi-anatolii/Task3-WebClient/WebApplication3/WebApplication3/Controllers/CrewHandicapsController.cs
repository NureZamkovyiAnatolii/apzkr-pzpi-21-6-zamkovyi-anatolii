using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class CrewHandicapsController : Controller
    {
        private readonly WebApplication3Context _context;

        public CrewHandicapsController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: CrewHandicaps
        public async Task<IActionResult> Index()
        {
              return _context.CrewHandicap != null ? 
                          View(await _context.CrewHandicap.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.CrewHandicap'  is null.");
        }

        // GET: CrewHandicaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CrewHandicap == null)
            {
                return NotFound();
            }

            var crewHandicap = await _context.CrewHandicap
                .FirstOrDefaultAsync(m => m.CrewHandicapId == id);
            if (crewHandicap == null)
            {
                return NotFound();
            }

            return View(crewHandicap);
        }

        // GET: CrewHandicaps/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRandomStartNumbers(int raceWithHandicapId)
        {
            var random = new Random();

            // Отримуємо всі CrewHandicap з заданим RaceWithHandicapId
            var crewHandicaps = await _context.CrewHandicap
                .Where(ch => ch.RaceWithHandicapId == raceWithHandicapId)
                .ToListAsync();

            foreach (var crewHandicap in crewHandicaps)
            {
                // Встановлюємо випадковий стартовий номер
                crewHandicap.StartNumber = random.Next(1, 1000); // або будь-який інший діапазон, що вам потрібен
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // POST: CrewHandicaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CrewHandicapId,BoatType,CompetitionId,RaceWithHandicapId,StartNumber")] CrewHandicap crewHandicap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crewHandicap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crewHandicap);
        }

        // GET: CrewHandicaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CrewHandicap == null)
            {
                return NotFound();
            }

            var crewHandicap = await _context.CrewHandicap.FindAsync(id);
            if (crewHandicap == null)
            {
                return NotFound();
            }
            return View(crewHandicap);
        }

        // POST: CrewHandicaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CrewHandicapId,BoatType,CompetitionId,RaceWithHandicapId,StartNumber")] CrewHandicap crewHandicap)
        {
            if (id != crewHandicap.CrewHandicapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crewHandicap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewHandicapExists(crewHandicap.CrewHandicapId))
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
            return View(crewHandicap);
        }

        // GET: CrewHandicaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CrewHandicap == null)
            {
                return NotFound();
            }

            var crewHandicap = await _context.CrewHandicap
                .FirstOrDefaultAsync(m => m.CrewHandicapId == id);
            if (crewHandicap == null)
            {
                return NotFound();
            }

            return View(crewHandicap);
        }

        // POST: CrewHandicaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CrewHandicap == null)
            {
                return Problem("Entity set 'WebApplication3Context.CrewHandicap'  is null.");
            }
            var crewHandicap = await _context.CrewHandicap.FindAsync(id);
            if (crewHandicap != null)
            {
                _context.CrewHandicap.Remove(crewHandicap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrewHandicapExists(int id)
        {
          return (_context.CrewHandicap?.Any(e => e.CrewHandicapId == id)).GetValueOrDefault();
        }
    }
}
