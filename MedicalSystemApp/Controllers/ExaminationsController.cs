using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;
using MedicalSystemApp.Repositories;

namespace MedicalSystemApp.Controllers
{
    public class ExaminationsController : Controller
    {
        private readonly RepositoryFactory _factory;
        private readonly IExaminationRepository _examRepo;
        private readonly IPatientRepository _patientRepo;

        public ExaminationsController(RepositoryFactory factory)
        {
            _factory = factory;
            _examRepo = _factory.CreateExaminationRepository();
            _patientRepo = _factory.CreatePatientRepository(); // to load patients
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _examRepo.GetAllAsync();
            return View(exams);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exam = await _examRepo.GetByIdAsync(id);
            if (exam == null)
                return NotFound();

            return View(exam);
        }

        // GET: Examinations/Create
        public async Task<IActionResult> Create()
        {
            var patients = await _patientRepo.GetAllAsync();
            ViewBag.Patients = patients;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Examination exam)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientRepo.GetAllAsync();
                return View(exam);
            }

            await _examRepo.AddAsync(exam);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exam = await _examRepo.GetByIdAsync(id);
            if (exam == null)
                return NotFound();

            // load patients
            ViewBag.Patients = await _patientRepo.GetAllAsync();
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Examination exam)
        {
            if (id != exam.Id)
                return BadRequest("Examination ID mismatch");

            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientRepo.GetAllAsync();
                return View(exam);
            }

            try
            {
                await _examRepo.UpdateAsync(exam);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExaminationExists(exam.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _examRepo.GetByIdAsync(id);
            if (exam == null)
                return NotFound();

            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _examRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ExaminationExists(int id)
        {
            return (await _examRepo.GetByIdAsync(id)) != null;
        }
    }
}
