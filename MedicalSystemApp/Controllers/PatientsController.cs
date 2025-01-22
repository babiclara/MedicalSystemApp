using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;
using MedicalSystemApp.Repositories;

namespace MedicalSystemApp.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly RepositoryFactory _factory;
        private readonly IPatientRepository _patientRepo;

        // Inject the RepositoryFactory
        public PatientsController(RepositoryFactory factory)
        {
            _factory = factory;
            // We obtain an instance of PatientRepository here:
            _patientRepo = _factory.CreatePatientRepository();
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            // Instead of _context.Patients, we now use the repository
            var patients = await _patientRepo.GetAllAsync();
            return View(patients);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _patientRepo.GetByIdAsync(id.Value);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            await _patientRepo.AddAsync(patient);
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _patientRepo.GetByIdAsync(id.Value);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.Id)
                return BadRequest("Mismatched Patient ID");

            if (!ModelState.IsValid)
                return View(patient);

            try
            {
                await _patientRepo.UpdateAsync(patient);
            }
            catch (DbUpdateConcurrencyException)
            {
                // We'll do a quick check to see if the patient still exists
                var exists = await PatientExists(patient.Id);
                if (!exists)
                    return NotFound();
                else
                    throw; // some other concurrency issue
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _patientRepo.GetByIdAsync(id.Value);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _patientRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Example "Search" or "Find" method if needed:
        public async Task<IActionResult> Search(string q)
        {
            var results = await _patientRepo.SearchAsync(q);
            return View("Index", results);
        }

        // Helper method to check if a patient exists
        private async Task<bool> PatientExists(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            return (patient != null);
        }
    }
}
