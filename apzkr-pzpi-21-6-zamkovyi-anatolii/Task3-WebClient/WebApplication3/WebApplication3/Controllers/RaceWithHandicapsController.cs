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
    public class RaceWithHandicapsController : Controller
    {
        private readonly WebApplication3Context _context;

        public RaceWithHandicapsController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: RaceWithHandicaps
        public async Task<IActionResult> Index()
        {
              return _context.RaceWithHandicap != null ? 
                          View(await _context.RaceWithHandicap.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.RaceWithHandicap'  is null.");
        }

        // GET: RaceWithHandicaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RaceWithHandicap == null)
            {
                return NotFound();
            }

            var raceWithHandicap = await _context.RaceWithHandicap
                .FirstOrDefaultAsync(m => m.RaceWithHandicapId == id);
            if (raceWithHandicap == null)
            {
                return NotFound();
            }

            return View(raceWithHandicap);
        }

        // GET: RaceWithHandicaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RaceWithHandicaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceWithHandicapId,StartTime,CompetitionId,Stage,HandicapDuration")] RaceWithHandicap raceWithHandicap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceWithHandicap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(raceWithHandicap);
        }

        // GET: RaceWithHandicaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RaceWithHandicap == null)
            {
                return NotFound();
            }

            var raceWithHandicap = await _context.RaceWithHandicap.FindAsync(id);
            if (raceWithHandicap == null)
            {
                return NotFound();
            }
            return View(raceWithHandicap);
        }

        // POST: RaceWithHandicaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceWithHandicapId,StartTime,CompetitionId,Stage,HandicapDuration")] RaceWithHandicap raceWithHandicap)
        {
            if (id != raceWithHandicap.RaceWithHandicapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceWithHandicap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceWithHandicapExists(raceWithHandicap.RaceWithHandicapId))
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
            return View(raceWithHandicap);
        }

        // GET: RaceWithHandicaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RaceWithHandicap == null)
            {
                return NotFound();
            }

            var raceWithHandicap = await _context.RaceWithHandicap
                .FirstOrDefaultAsync(m => m.RaceWithHandicapId == id);
            if (raceWithHandicap == null)
            {
                return NotFound();
            }

            return View(raceWithHandicap);
        }

        // POST: RaceWithHandicaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RaceWithHandicap == null)
            {
                return Problem("Entity set 'WebApplication3Context.RaceWithHandicap'  is null.");
            }
            var raceWithHandicap = await _context.RaceWithHandicap.FindAsync(id);
            if (raceWithHandicap != null)
            {
                _context.RaceWithHandicap.Remove(raceWithHandicap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceWithHandicapExists(int id)
        {
          return (_context.RaceWithHandicap?.Any(e => e.RaceWithHandicapId == id)).GetValueOrDefault();
        }
    }
}
