using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduOnline.DAL;
using EduOnline.DAL.Entities;

namespace EduOnline.Controllers
{
    public class CountriesController : Controller
    {
        #region Dependencies & Properties
        private readonly DatabaseContext _context;

        public CountriesController(DatabaseContext context)
        {
            _context = context;
        }

        #endregion

        #region Country Actions
        public async Task<IActionResult> Index()
        {
              return View(await _context.Countries.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedDate,ModifiedDate")] Country country)
        {
            if (ModelState.IsValid)
            {
                country.Id = Guid.NewGuid();
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id,CreatedDate,ModifiedDate")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            return View(country);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DatabaseContext.Countries'  is null.");
            }
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(Guid id)
        {
          return _context.Countries.Any(e => e.Id == id);
        }

        #endregion
    }
}
