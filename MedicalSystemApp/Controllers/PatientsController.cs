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

        public async Task<IActionResult> Index()
        {
            var patients = await _patientRepo.GetAllAsync();
            return View(patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            await _patientRepo.AddAsync(patient);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            await _patientRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ExportCsv()
        {
            var patients = await _patientRepo.GetAllAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Id,FirstName,LastName,OIB,DateOfBirth,Gender");
            foreach (var p in patients)
            {
                sb.AppendLine($"{p.Id},{p.FirstName},{p.LastName},{p.OIB},{p.DateOfBirth:yyyy-MM-dd},{p.Gender}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"Patients_{DateTime.Now:yyyyMMdd}.csv";
            return File(csvBytes, "text/csv", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction(nameof(Index));
            }

            var results = await _patientRepo.SearchAsync(query);
            return View("Index", results);
        }

        private async Task<bool> PatientExists(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            return (patient != null);
        }
    }
}
