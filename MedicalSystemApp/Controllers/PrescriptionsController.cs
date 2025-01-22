using MedicalSystemApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApp.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly IPrescriptionRepository _prescriptionRepo;

        // Or if you want to use the Factory:
        // private readonly RepositoryFactory _factory;
        // public PrescriptionsController(RepositoryFactory factory) {
        //     _factory = factory;
        //     _prescriptionRepo = factory.CreatePrescriptionRepository();
        // }

        public PrescriptionsController(IPrescriptionRepository repo)
        {
            _prescriptionRepo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync();
            return View(prescriptions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _prescriptionRepo.GetByIdAsync(id);
            if (prescription == null) return NotFound();
            return View(prescription);
        }

        // Additional CRUD actions (Create, Edit, Delete) are similar to the Patient example.
    }

}
