using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly MedicalDbContext _db;

        public PrescriptionRepository(MedicalDbContext db)
        {
            _db = db;
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            // If you want to include Patient data automatically:
            // return await _db.Prescriptions
            //     .Include(p => p.Patient)
            //     .FirstOrDefaultAsync(pr => pr.Id == id);

            // Otherwise (lazy loading or no related data needed):
            return await _db.Prescriptions.FindAsync(id);
        }

        public async Task<List<Prescription>> GetAllAsync()
        {
            // Optionally .Include(p => p.Patient) if you want
            return await _db.Prescriptions.ToListAsync();
        }

        public async Task AddAsync(Prescription prescription)
        {
            await _db.Prescriptions.AddAsync(prescription);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            _db.Prescriptions.Update(prescription);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prescription = await _db.Prescriptions.FindAsync(id);
            if (prescription != null)
            {
                _db.Prescriptions.Remove(prescription);
                await _db.SaveChangesAsync();
            }
        }
    }
}
