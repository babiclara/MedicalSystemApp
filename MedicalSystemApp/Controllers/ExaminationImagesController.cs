using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;
using MedicalSystemApp.Repositories;

namespace MedicalSystemApp.Controllers
{
    public class ExaminationImagesController : Controller
    {
        private readonly RepositoryFactory _factory;
        private readonly IExaminationImageRepository _imageRepo;
        private readonly IExaminationRepository _examRepo; 

        public ExaminationImagesController(RepositoryFactory factory)
        {
            _factory = factory;
            _imageRepo = _factory.CreateExaminationImageRepository();
            _examRepo = _factory.CreateExaminationRepository();
        }

        public async Task<IActionResult> Index()
        {
            var images = await _imageRepo.GetAllAsync();
            return View(images);
        }

        public async Task<IActionResult> Details(int id)
        {
            var image = await _imageRepo.GetByIdAsync(id);
            if (image == null)
                return NotFound();
            return View(image);
        }

        public async Task<IActionResult> Create()
        {
            var exams = await _examRepo.GetAllAsync();
            ViewBag.Examinations = exams;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExaminationImage image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Examinations = await _examRepo.GetAllAsync();
                return View(image);
            }

            await _imageRepo.AddAsync(image);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var image = await _imageRepo.GetByIdAsync(id);
            if (image == null)
                return NotFound();

            var exams = await _examRepo.GetAllAsync();
            ViewBag.Examinations = exams;

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExaminationImage image)
        {
            if (id != image.Id)
                return BadRequest("Image ID mismatch");

            if (!ModelState.IsValid)
            {
                ViewBag.Examinations = await _examRepo.GetAllAsync();
                return View(image);
            }

            try
            {
                await _imageRepo.UpdateAsync(image);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExaminationImageExists(image.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var image = await _imageRepo.GetByIdAsync(id);
            if (image == null)
                return NotFound();

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _imageRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ExaminationImageExists(int id)
        {
            var existing = await _imageRepo.GetByIdAsync(id);
            return (existing != null);
        }

        public async Task<IActionResult> ForExamination(int examId)
        {
            var images = await _imageRepo.GetByExaminationIdAsync(examId);
            return View("Index", images);
        }
    }
}
