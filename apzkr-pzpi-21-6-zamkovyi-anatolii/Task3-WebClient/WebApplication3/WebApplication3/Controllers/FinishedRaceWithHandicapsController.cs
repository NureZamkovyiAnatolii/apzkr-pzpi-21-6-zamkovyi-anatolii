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
    public class FinishedRaceWithHandicapsController : Controller
    {
        private readonly WebApplication3Context _context;

        public FinishedRaceWithHandicapsController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: FinishedRaceWithHandicaps
        public async Task<IActionResult> Index()
        {
              return _context.FinishedRaceWithHandicap != null ? 
                          View(await _context.FinishedRaceWithHandicap.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.FinishedRaceWithHandicap'  is null.");
        }

        // GET: FinishedRaceWithHandicaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FinishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            var finishedRaceWithHandicap = await _context.FinishedRaceWithHandicap
                .FirstOrDefaultAsync(m => m.FinishedRaceWithHandicapId == id);
            if (finishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            return View(finishedRaceWithHandicap);
        }

        // GET: FinishedRaceWithHandicaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FinishedRaceWithHandicaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FinishedRaceWithHandicapId,RaceWithHandicapId,FinishTime")] FinishedRaceWithHandicap finishedRaceWithHandicap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finishedRaceWithHandicap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(finishedRaceWithHandicap);
        }

        // GET: FinishedRaceWithHandicaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FinishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            var finishedRaceWithHandicap = await _context.FinishedRaceWithHandicap.FindAsync(id);
            if (finishedRaceWithHandicap == null)
            {
                return NotFound();
            }
            return View(finishedRaceWithHandicap);
        }

        // POST: FinishedRaceWithHandicaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FinishedRaceWithHandicapId,RaceWithHandicapId,FinishTime")] FinishedRaceWithHandicap finishedRaceWithHandicap)
        {
            if (id != finishedRaceWithHandicap.FinishedRaceWithHandicapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finishedRaceWithHandicap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinishedRaceWithHandicapExists(finishedRaceWithHandicap.FinishedRaceWithHandicapId))
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
            return View(finishedRaceWithHandicap);
        }

        // GET: FinishedRaceWithHandicaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FinishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            var finishedRaceWithHandicap = await _context.FinishedRaceWithHandicap
                .FirstOrDefaultAsync(m => m.FinishedRaceWithHandicapId == id);
            if (finishedRaceWithHandicap == null)
            {
                return NotFound();
            }

            return View(finishedRaceWithHandicap);
        }

        // POST: FinishedRaceWithHandicaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FinishedRaceWithHandicap == null)
            {
                return Problem("Entity set 'WebApplication3Context.FinishedRaceWithHandicap'  is null.");
            }
            var finishedRaceWithHandicap = await _context.FinishedRaceWithHandicap.FindAsync(id);
            if (finishedRaceWithHandicap != null)
            {
                _context.FinishedRaceWithHandicap.Remove(finishedRaceWithHandicap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinishedRaceWithHandicapExists(int id)
        {
          return (_context.FinishedRaceWithHandicap?.Any(e => e.FinishedRaceWithHandicapId == id)).GetValueOrDefault();
        }
    }
}
