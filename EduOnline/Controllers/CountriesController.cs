﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduOnline.DAL;
using EduOnline.DAL.Entities;
using EduOnline.Models;

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
            return View(await _context.Countries
                .Include(c => c.States)
                .ToListAsync());
        }
                
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
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
        public async Task<IActionResult> Edit(Guid id, Country country)
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
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(country);
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

        public async Task<IActionResult> Delete(Guid? countryId)
        {
            if (countryId == null || _context.Countries == null)
            {
                return NotFound();
            }

            Country country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == countryId);
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
                return Problem("Entity set 'DatabaseContext.Countries' is null.");
            }
            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id);
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

        #region State Actions
        [HttpGet]
        public async Task<IActionResult> AddState(Guid? countryId)
        {
            if (countryId == null)
            {
                return NotFound();
            }

            Country country = await _context.Countries.FindAsync(countryId);
            if (country == null)
            {
                return NotFound();
            }

            StateViewModel stateViewModel = new()
            {
                CountryId = country.Id,
            };

            return View(stateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel stateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new State()
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FindAsync(stateViewModel.CountryId),
                        Name = stateViewModel.Name,
                        CreatedDate = stateViewModel.CreatedDate,
                        ModifiedDate = DateTime.Now,
                    };

                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = stateViewModel.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Dpto/Estado con el mismo nombre en este país.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(stateViewModel);
        }

        #endregion
    }
}
