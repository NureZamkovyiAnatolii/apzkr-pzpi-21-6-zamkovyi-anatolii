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
    public class CrewRowersController : Controller
    {
        private readonly WebApplication3Context _context;

        public CrewRowersController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: CrewRowers
        public async Task<IActionResult> Index()
        {
              return _context.CrewRower != null ? 
                          View(await _context.CrewRower.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.CrewRower'  is null.");
        }

        // GET: CrewRowers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CrewRower == null)
            {
                return NotFound();
            }

            var crewRower = await _context.CrewRower
                .FirstOrDefaultAsync(m => m.CrewRowerId == id);
            if (crewRower == null)
            {
                return NotFound();
            }

            return View(crewRower);
        }

        // GET: CrewRowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CrewRowers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CrewRowerId,AthleteId,CrewId")] CrewRower crewRower)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crewRower);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crewRower);
        }

        // GET: CrewRowers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CrewRower == null)
            {
                return NotFound();
            }

            var crewRower = await _context.CrewRower.FindAsync(id);
            if (crewRower == null)
            {
                return NotFound();
            }
            return View(crewRower);
        }

        // POST: CrewRowers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CrewRowerId,AthleteId,CrewId")] CrewRower crewRower)
        {
            if (id != crewRower.CrewRowerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crewRower);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewRowerExists(crewRower.CrewRowerId))
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
            return View(crewRower);
        }

        // GET: CrewRowers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CrewRower == null)
            {
                return NotFound();
            }

            var crewRower = await _context.CrewRower
                .FirstOrDefaultAsync(m => m.CrewRowerId == id);
            if (crewRower == null)
            {
                return NotFound();
            }

            return View(crewRower);
        }

        // POST: CrewRowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CrewRower == null)
            {
                return Problem("Entity set 'WebApplication3Context.CrewRower'  is null.");
            }
            var crewRower = await _context.CrewRower.FindAsync(id);
            if (crewRower != null)
            {
                _context.CrewRower.Remove(crewRower);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrewRowerExists(int id)
        {
          return (_context.CrewRower?.Any(e => e.CrewRowerId == id)).GetValueOrDefault();
        }
    }
}
