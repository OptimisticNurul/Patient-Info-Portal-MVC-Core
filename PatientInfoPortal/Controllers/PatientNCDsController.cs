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
    public class PatientNCDsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientNCDsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PatientNCDs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientNCDs.Include(p => p.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PatientNCDs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientNCDs == null)
            {
                return NotFound();
            }

            var patientNCD = await _context.PatientNCDs
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientNCD == null)
            {
                return NotFound();
            }

            return View(patientNCD);
        }

        // GET: PatientNCDs/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: PatientNCDs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,NCD")] PatientNCD patientNCD)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientNCD);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", patientNCD.PatientId);
            return View(patientNCD);
        }

        // GET: PatientNCDs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientNCDs == null)
            {
                return NotFound();
            }

            var patientNCD = await _context.PatientNCDs.FindAsync(id);
            if (patientNCD == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", patientNCD.PatientId);
            return View(patientNCD);
        }

        // POST: PatientNCDs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,NCD")] PatientNCD patientNCD)
        {
            if (id != patientNCD.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientNCD);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientNCDExists(patientNCD.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", patientNCD.PatientId);
            return View(patientNCD);
        }

        // GET: PatientNCDs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientNCDs == null)
            {
                return NotFound();
            }

            var patientNCD = await _context.PatientNCDs
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientNCD == null)
            {
                return NotFound();
            }

            return View(patientNCD);
        }

        // POST: PatientNCDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientNCDs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PatientNCDs'  is null.");
            }
            var patientNCD = await _context.PatientNCDs.FindAsync(id);
            if (patientNCD != null)
            {
                _context.PatientNCDs.Remove(patientNCD);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientNCDExists(int id)
        {
          return (_context.PatientNCDs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
