using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public class ExaminationImageRepository : IExaminationImageRepository
    {
        private readonly MedicalDbContext _db;

        public ExaminationImageRepository(MedicalDbContext db)
        {
            _db = db;
        }

        public async Task<ExaminationImage?> GetByIdAsync(int id)
        {
            // Optionally .Include(ei => ei.Examination) if you want to auto-load the related Examination
            // return await _db.ExaminationImages
            //     .Include(ei => ei.Examination)
            //     .FirstOrDefaultAsync(ei => ei.Id == id);

            return await _db.ExaminationImages.FindAsync(id);
        }

        public async Task<List<ExaminationImage>> GetAllAsync()
        {
            // Possibly .Include(ei => ei.Examination) for related data
            return await _db.ExaminationImages.ToListAsync();
        }

        public async Task AddAsync(ExaminationImage image)
        {
            await _db.ExaminationImages.AddAsync(image);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExaminationImage image)
        {
            _db.ExaminationImages.Update(image);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.ExaminationImages.FindAsync(id);
            if (existing != null)
            {
                _db.ExaminationImages.Remove(existing);
                await _db.SaveChangesAsync();
            }
        }

        // Implementation of the optional method
        public async Task<List<ExaminationImage>> GetByExaminationIdAsync(int examId)
        {
            // Optionally .Include(ei => ei.Examination)
            return await _db.ExaminationImages
                .Where(ei => ei.ExaminationId == examId)
                .ToListAsync();
        }
    }
}
