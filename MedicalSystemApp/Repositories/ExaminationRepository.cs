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

            return await _db.Examinations.FindAsync(id);
        }

        public async Task<List<Examination>> GetAllAsync()
        {
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

        public async Task<List<Examination>> GetByTypeAsync(string examType)
        {
            return await _db.Examinations
                .Where(e => e.ExaminationType == examType)
                .ToListAsync();
        }

        public async Task<List<Examination>> GetByPatientIdAsync(int patientId)
        {
            return await _db.Examinations
                .Where(e => e.PatientId == patientId)
                .ToListAsync();
        }
    }
}
