using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatientInfoPortal.Data;
using PatientInfoPortal.Models;

namespace PatientInfoPortal.Controllers
{
    public class PatientAllergiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientAllergiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PatientAllergies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientAllergies.Include(p => p.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PatientAllergies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientAllergies == null)
            {
                return NotFound();
            }

            var patientAllergy = await _context.PatientAllergies
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientAllergy == null)
            {
                return NotFound();
            }

            return View(patientAllergy);
        }

        // GET: PatientAllergies/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: PatientAllergies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,Allergy")] PatientAllergy patientAllergy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientAllergy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", patientAllergy.PatientId);
            return View(patientAllergy);
        }

        // GET: PatientAllergies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientAllergies == null)
            {
                return NotFound();
            }

            var patientAllergy = await _context.PatientAllergies.FindAsync(id);
            if (patientAllergy == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", patientAllergy.PatientId);
            return View(patientAllergy);
        }

        // POST: PatientAllergies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,Allergy")] PatientAllergy patientAllergy)
        {
            if (id != patientAllergy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientAllergy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientAllergyExists(patientAllergy.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", patientAllergy.PatientId);
            return View(patientAllergy);
        }

        // GET: PatientAllergies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientAllergies == null)
            {
                return NotFound();
            }

            var patientAllergy = await _context.PatientAllergies
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientAllergy == null)
            {
                return NotFound();
            }

            return View(patientAllergy);
        }

        // POST: PatientAllergies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientAllergies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PatientAllergies'  is null.");
            }
            var patientAllergy = await _context.PatientAllergies.FindAsync(id);
            if (patientAllergy != null)
            {
                _context.PatientAllergies.Remove(patientAllergy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientAllergyExists(int id)
        {
          return (_context.PatientAllergies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
