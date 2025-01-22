using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MedicalDbContext _db;

        public PatientRepository(MedicalDbContext db)
        {
            _db = db;
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _db.Patients.FindAsync(id);
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _db.Patients
                .OrderBy(p => p.LastName)
                .ToListAsync();
        }

        public async Task AddAsync(Patient patient)
        {
            await _db.Patients.AddAsync(patient);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _db.Patients.Update(patient);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            if (patient != null)
            {
                _db.Patients.Remove(patient);
                await _db.SaveChangesAsync();
            }
        }

        // Additional custom method: search by LastName or OIB
        public async Task<List<Patient>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Patient>();

            searchTerm = searchTerm.ToLower();
            return await _db.Patients
                .Where(p => p.LastName.ToLower().Contains(searchTerm) ||
                            p.OIB.ToLower().Contains(searchTerm))
                .OrderBy(p => p.LastName)
                .ToListAsync();
        }
    }
}
