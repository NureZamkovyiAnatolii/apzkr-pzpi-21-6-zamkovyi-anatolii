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
    public class ResultHandicapsController : Controller
    {
        private readonly WebApplication3Context _context;

        public ResultHandicapsController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: ResultHandicaps
        public async Task<IActionResult> Index()
        {
              return _context.ResultHandicap != null ? 
                          View(await _context.ResultHandicap.ToListAsync()) :
                          Problem("Entity set 'WebApplication3Context.ResultHandicap'  is null.");
        }

        // GET: ResultHandicaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ResultHandicap == null)
            {
                return NotFound();
            }

            var resultHandicap = await _context.ResultHandicap
                .FirstOrDefaultAsync(m => m.ResultHandicapId == id);
            if (resultHandicap == null)
            {
                return NotFound();
            }

            return View(resultHandicap);
        }

        // GET: ResultHandicaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResultHandicaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResultHandicapId,CrewHandicapId,TimeTaken")] ResultHandicap resultHandicap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultHandicap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultHandicap);
        }

        // GET: ResultHandicaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ResultHandicap == null)
            {
                return NotFound();
            }

            var resultHandicap = await _context.ResultHandicap.FindAsync(id);
            if (resultHandicap == null)
            {
                return NotFound();
            }
            return View(resultHandicap);
        }

        // POST: ResultHandicaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResultHandicapId,CrewHandicapId,TimeTaken")] ResultHandicap resultHandicap)
        {
            if (id != resultHandicap.ResultHandicapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultHandicap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultHandicapExists(resultHandicap.ResultHandicapId))
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
            return View(resultHandicap);
        }

        // GET: ResultHandicaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ResultHandicap == null)
            {
                return NotFound();
            }

            var resultHandicap = await _context.ResultHandicap
                .FirstOrDefaultAsync(m => m.ResultHandicapId == id);
            if (resultHandicap == null)
            {
                return NotFound();
            }

            return View(resultHandicap);
        }

        // POST: ResultHandicaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ResultHandicap == null)
            {
                return Problem("Entity set 'WebApplication3Context.ResultHandicap'  is null.");
            }
            var resultHandicap = await _context.ResultHandicap.FindAsync(id);
            if (resultHandicap != null)
            {
                _context.ResultHandicap.Remove(resultHandicap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultHandicapExists(int id)
        {
          return (_context.ResultHandicap?.Any(e => e.ResultHandicapId == id)).GetValueOrDefault();
        }
    }
}
