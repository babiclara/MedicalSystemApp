﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;
using MedicalSystemApp.Repositories;

namespace MedicalSystemApp.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly RepositoryFactory _factory;
        private readonly IMedicalRecordRepository _recordRepo;
        private readonly IPatientRepository _patientRepo;

        public MedicalRecordsController(RepositoryFactory factory)
        {
            _factory = factory;
            _recordRepo = _factory.CreateMedicalRecordRepository();
            _patientRepo = _factory.CreatePatientRepository(); // for listing patients in dropdown
        }

        public async Task<IActionResult> Index()
        {
            var records = await _recordRepo.GetAllAsync();
            return View(records);
        }

        public async Task<IActionResult> Details(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            return View(record);
        }

        // GET: MedicalRecords/Create
        public async Task<IActionResult> Create()
        {
            // Load all patients for dropdown
            var patients = await _patientRepo.GetAllAsync();
            ViewBag.Patients = patients;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecord record)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientRepo.GetAllAsync();
                return View(record);
            }

            // Convert to UTC and set kind
            record.StartDate = DateTime.SpecifyKind(record.StartDate, DateTimeKind.Utc);
            if (record.EndDate.HasValue)
            {
                record.EndDate = DateTime.SpecifyKind(record.EndDate.Value, DateTimeKind.Utc);
            }

            await _recordRepo.AddAsync(record);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            // Load patients for dropdown in case user wants to change PatientId
            var patients = await _patientRepo.GetAllAsync();
            ViewBag.Patients = patients;

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalRecord record)
        {
            if (id != record.Id)
                return BadRequest("Mismatched record ID");

            if (!ModelState.IsValid)
            {
                // Reload patients
                ViewBag.Patients = await _patientRepo.GetAllAsync();
                return View(record);
            }

            try
            {
                await _recordRepo.UpdateAsync(record);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MedicalRecordExists(record.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _recordRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MedicalRecordExists(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            return record != null;
        }
    }
}
