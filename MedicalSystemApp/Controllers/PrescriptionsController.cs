using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;
using MedicalSystemApp.Repositories;

namespace MedicalSystemApp.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly RepositoryFactory _factory;
        private readonly IPrescriptionRepository _prescriptionRepo;
        private readonly IPatientRepository _patientRepo;

        public PrescriptionsController(RepositoryFactory factory)
        {
            _factory = factory;
            _prescriptionRepo = _factory.CreatePrescriptionRepository();
            _patientRepo = _factory.CreatePatientRepository(); 
        }

        public async Task<IActionResult> Index()
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync();
            return View(prescriptions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _prescriptionRepo.GetByIdAsync(id);
            if (prescription == null)
                return NotFound();

            return View(prescription);
        }

        public async Task<IActionResult> Create()
        {
            var patients = await _patientRepo.GetAllAsync();
            ViewBag.Patients = patients;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientRepo.GetAllAsync();
                return View(prescription);
            }

            await _prescriptionRepo.AddAsync(prescription);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var prescription = await _prescriptionRepo.GetByIdAsync(id);
            if (prescription == null)
                return NotFound();

            ViewBag.Patients = await _patientRepo.GetAllAsync();
            return View(prescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prescription prescription)
        {
            if (id != prescription.Id)
                return BadRequest("Prescription ID mismatch");

            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientRepo.GetAllAsync();
                return View(prescription);
            }

            try
            {
                await _prescriptionRepo.UpdateAsync(prescription);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PrescriptionExists(prescription.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prescription = await _prescriptionRepo.GetByIdAsync(id);
            if (prescription == null)
                return NotFound();

            return View(prescription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prescriptionRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PrescriptionExists(int id)
        {
            var prescription = await _prescriptionRepo.GetByIdAsync(id);
            return prescription != null;
        }
    }
}
