using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientInfoPortal.Data;
using PatientInfoPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PatientInfoPortal.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients
                .Include(p => p.PatientNCDs)
                .Include(p => p.PatientAllergies)
                .ToListAsync();
            return View(patients);
        }

        public IActionResult Create()
        {
            ViewBag.NCDs = new List<string> { "Asthma", "Cancer", "Disorders of ear", "Disorder of eye", "Mental illness", "Oral health problems" };
            ViewBag.Allergies = new List<string> { "Drugs - Penicillin", "Drugs - Others", "Animals", "Food", "Oinments", "Plant", "Sprays", "Others", "No Allergies" };
            return View(new Patient());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient, List<string> selectedNCDs, List<string> selectedAllergies)
        {
            if (ModelState.IsValid)
            {
                foreach (var ncd in selectedNCDs)
                {
                    patient.PatientNCDs.Add(new PatientNCD { NCD = ncd });
                }

                foreach (var allergy in selectedAllergies)
                {
                    patient.PatientAllergies.Add(new PatientAllergy { Allergy = allergy });
                }

                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.NCDs = new List<string> { "Asthma", "Cancer", "Disorders of ear", "Disorder of eye", "Mental illness", "Oral health problems" };
            ViewBag.Allergies = new List<string> { "Drugs - Penicillin", "Drugs - Others", "Animals", "Food", "Oinments", "Plant", "Sprays", "Others", "No Allergies" };
            return View(patient);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PatientNCDs)
                .Include(p => p.PatientAllergies)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.NCDs = new List<string> { "Asthma", "Cancer", "Disorders of ear", "Disorder of eye", "Mental illness", "Oral health problems" };
            ViewBag.Allergies = new List<string> { "Drugs - Penicillin", "Drugs - Others", "Animals", "Food", "Oinments", "Plant", "Sprays", "Others", "No Allergies" };

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DiseaseName,Epilepsy")] Patient patient, List<string> SelectedNCDs, List<string> SelectedAllergies)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find existing patient
                    var existingPatient = await _context.Patients
                        .Include(p => p.PatientNCDs)
                        .Include(p => p.PatientAllergies)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingPatient == null)
                    {
                        return NotFound();
                    }

                    // Update basic fields
                    existingPatient.Name = patient.Name;
                    existingPatient.DiseaseName = patient.DiseaseName;
                    existingPatient.Epilepsy = patient.Epilepsy;

                    // Update NCDs
                    _context.PatientNCDs.RemoveRange(existingPatient.PatientNCDs);
                    foreach (var ncd in SelectedNCDs)
                    {
                        _context.PatientNCDs.Add(new PatientNCD { PatientId = id, NCD = ncd });
                    }

                    // Update Allergies
                    _context.PatientAllergies.RemoveRange(existingPatient.PatientAllergies);
                    foreach (var allergy in SelectedAllergies)
                    {
                        _context.PatientAllergies.Add(new PatientAllergy { PatientId = id, Allergy = allergy });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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

            ViewBag.NCDs = new List<string> { "Asthma", "Cancer", "Disorders of ear", "Disorder of eye", "Mental illness", "Oral health problems" };
            ViewBag.Allergies = new List<string> { "Drugs - Penicillin", "Drugs - Others", "Animals", "Food", "Oinments", "Plant", "Sprays", "Others", "No Allergies" };

            return View(patient);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PatientNCDs)
                .Include(p => p.PatientAllergies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PatientNCDs)
                .Include(p => p.PatientAllergies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
