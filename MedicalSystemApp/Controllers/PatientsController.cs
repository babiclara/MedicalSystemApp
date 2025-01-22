using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;
using MedicalSystemApp.Repositories;
using System.Text;

namespace MedicalSystemApp.Controllers
{
    public class PatientsController : Controller
    {
        private readonly RepositoryFactory _factory;
        private readonly IPatientRepository _patientRepo;

        public PatientsController(RepositoryFactory factory)
        {
            _factory = factory;
            _patientRepo = _factory.CreatePatientRepository();
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var patients = await _patientRepo.GetAllAsync();
            return View(patients);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

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
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

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
                if (!await PatientExists(patient.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Optionally retrieve the patient for a "DeleteConfirmed" page:
            // var patient = await _patientRepo.GetByIdAsync(id);
            // if (patient == null) return NotFound();
            await _patientRepo.DeleteAsync(id);

            // return View("DeleteConfirmed", patient); // if you want to show a separate "deleted" page
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/ExportCsv
        [HttpGet]
        public async Task<IActionResult> ExportCsv()
        {
            var patients = await _patientRepo.GetAllAsync();

            // Build CSV in memory
            var sb = new StringBuilder();
            sb.AppendLine("Id,FirstName,LastName,OIB,DateOfBirth,Gender");
            foreach (var p in patients)
            {
                sb.AppendLine($"{p.Id},{p.FirstName},{p.LastName},{p.OIB},{p.DateOfBirth:yyyy-MM-dd},{p.Gender}");
            }

            // Return CSV as file
            var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"Patients_{DateTime.Now:yyyyMMdd}.csv";
            return File(csvBytes, "text/csv", fileName);
        }

        // GET: Patients/Search?query=someText
        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            // If your repository doesn't have a SearchAsync, create it. 
            // Example below (pseudocode):
            if (string.IsNullOrWhiteSpace(query))
            {
                // Return empty or all patients
                // return View("Index", new List<Patient>());
                return RedirectToAction(nameof(Index));
            }

            // Suppose your IPatientRepository has: Task<List<Patient>> SearchAsync(string text)
            var results = await _patientRepo.SearchAsync(query);
            return View("Index", results);
        }

        // Helper method to check existence
        private async Task<bool> PatientExists(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            return (patient != null);
        }
    }
}
