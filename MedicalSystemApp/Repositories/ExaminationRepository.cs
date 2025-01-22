using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public class ExaminationRepository : IExaminationRepository
    {
        private readonly MedicalDbContext _db;

        public ExaminationRepository(MedicalDbContext db)
        {
            _db = db;
        }

        public async Task<Examination?> GetByIdAsync(int id)
        {
            // If you want to include images and patient eagerly:
            // return await _db.Examinations
            //    .Include(e => e.Patient)
            //    .Include(e => e.ExaminationImages)
            //    .FirstOrDefaultAsync(e => e.Id == id);

            return await _db.Examinations.FindAsync(id);
        }

        public async Task<List<Examination>> GetAllAsync()
        {
            // Possibly .Include(e => e.Patient).Include(e => e.ExaminationImages)
            return await _db.Examinations.ToListAsync();
        }

        public async Task AddAsync(Examination exam)
        {
            await _db.Examinations.AddAsync(exam);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Examination exam)
        {
            _db.Examinations.Update(exam);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var exam = await _db.Examinations.FindAsync(id);
            if (exam != null)
            {
                _db.Examinations.Remove(exam);
                await _db.SaveChangesAsync();
            }
        }

        // Example: get by exam type
        public async Task<List<Examination>> GetByTypeAsync(string examType)
        {
            // e.g. "KRV", "GP", "CT", etc. 
            return await _db.Examinations
                .Where(e => e.ExaminationType == examType)
                .ToListAsync();
        }

        // Example: get by patient ID
        public async Task<List<Examination>> GetByPatientIdAsync(int patientId)
        {
            return await _db.Examinations
                .Where(e => e.PatientId == patientId)
                .ToListAsync();
        }
    }
}
