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
    public class CrewHandicapRowersController : Controller
    {
        private readonly WebApplication3Context _context;

        public CrewHandicapRowersController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: CrewHandicapRowers
        public async Task<IActionResult> Index()
        {
              return _context.CrewHandicapRower != null ? 
                          View(await _context.CrewHandicapRower.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.CrewHandicapRower'  is null.");
        }

        // GET: CrewHandicapRowers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CrewHandicapRower == null)
            {
                return NotFound();
            }

            var crewHandicapRower = await _context.CrewHandicapRower
                .FirstOrDefaultAsync(m => m.CrewHandicapRowerId == id);
            if (crewHandicapRower == null)
            {
                return NotFound();
            }

            return View(crewHandicapRower);
        }

        // GET: CrewHandicapRowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CrewHandicapRowers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CrewHandicapRowerId,AthleteId,CrewHandicapId")] CrewHandicapRower crewHandicapRower)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crewHandicapRower);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crewHandicapRower);
        }

        // GET: CrewHandicapRowers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CrewHandicapRower == null)
            {
                return NotFound();
            }

            var crewHandicapRower = await _context.CrewHandicapRower.FindAsync(id);
            if (crewHandicapRower == null)
            {
                return NotFound();
            }
            return View(crewHandicapRower);
        }

        // POST: CrewHandicapRowers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CrewHandicapRowerId,AthleteId,CrewHandicapId")] CrewHandicapRower crewHandicapRower)
        {
            if (id != crewHandicapRower.CrewHandicapRowerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crewHandicapRower);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewHandicapRowerExists(crewHandicapRower.CrewHandicapRowerId))
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
            return View(crewHandicapRower);
        }

        // GET: CrewHandicapRowers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CrewHandicapRower == null)
            {
                return NotFound();
            }

            var crewHandicapRower = await _context.CrewHandicapRower
                .FirstOrDefaultAsync(m => m.CrewHandicapRowerId == id);
            if (crewHandicapRower == null)
            {
                return NotFound();
            }

            return View(crewHandicapRower);
        }

        // POST: CrewHandicapRowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CrewHandicapRower == null)
            {
                return Problem("Entity set 'WebApplication3Context.CrewHandicapRower'  is null.");
            }
            var crewHandicapRower = await _context.CrewHandicapRower.FindAsync(id);
            if (crewHandicapRower != null)
            {
                _context.CrewHandicapRower.Remove(crewHandicapRower);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrewHandicapRowerExists(int id)
        {
          return (_context.CrewHandicapRower?.Any(e => e.CrewHandicapRowerId == id)).GetValueOrDefault();
        }
    }
}
