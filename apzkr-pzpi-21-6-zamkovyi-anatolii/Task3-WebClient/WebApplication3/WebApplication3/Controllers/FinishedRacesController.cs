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
    public class FinishedRacesController : Controller
    {
        private readonly WebApplication3Context _context;

        public FinishedRacesController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: FinishedRaces
        public async Task<IActionResult> Index()
        {
              return _context.FinishedRace != null ? 
                          View(await _context.FinishedRace.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.FinishedRace'  is null.");
        }

        public async Task<IActionResult> GetAPI()
        {
            return Ok("efef");
        }
        // GET: FinishedRaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FinishedRace == null)
            {
                return NotFound();
            }

            var finishedRace = await _context.FinishedRace
                .FirstOrDefaultAsync(m => m.FinishedRaceId == id);
            if (finishedRace == null)
            {
                return NotFound();
            }

            return View(finishedRace);
        }

        // GET: FinishedRaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FinishedRaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FinishedRaceId,RaceId,FinishTime")] FinishedRace finishedRace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finishedRace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(finishedRace);
        }

        // GET: FinishedRaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FinishedRace == null)
            {
                return NotFound();
            }

            var finishedRace = await _context.FinishedRace.FindAsync(id);
            if (finishedRace == null)
            {
                return NotFound();
            }
            return View(finishedRace);
        }

        // POST: FinishedRaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FinishedRaceId,RaceId,FinishTime")] FinishedRace finishedRace)
        {
            if (id != finishedRace.FinishedRaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finishedRace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinishedRaceExists(finishedRace.FinishedRaceId))
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
            return View(finishedRace);
        }

        // GET: FinishedRaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FinishedRace == null)
            {
                return NotFound();
            }

            var finishedRace = await _context.FinishedRace
                .FirstOrDefaultAsync(m => m.FinishedRaceId == id);
            if (finishedRace == null)
            {
                return NotFound();
            }

            return View(finishedRace);
        }

        // POST: FinishedRaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FinishedRace == null)
            {
                return Problem("Entity set 'WebApplication3Context.FinishedRace'  is null.");
            }
            var finishedRace = await _context.FinishedRace.FindAsync(id);
            if (finishedRace != null)
            {
                _context.FinishedRace.Remove(finishedRace);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinishedRaceExists(int id)
        {
          return (_context.FinishedRace?.Any(e => e.FinishedRaceId == id)).GetValueOrDefault();
        }
    }
}
