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
            return await _db.Prescriptions.FindAsync(id);
        }

        public async Task<List<Prescription>> GetAllAsync()
        {
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
