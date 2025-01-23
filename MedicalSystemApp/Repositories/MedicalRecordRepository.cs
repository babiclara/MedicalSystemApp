using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly MedicalDbContext _db;

        public MedicalRecordRepository(MedicalDbContext db)
        {
            _db = db;
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _db.MedicalRecords.FindAsync(id);
        }

        public async Task<List<MedicalRecord>> GetAllAsync()
        {
            return await _db.MedicalRecords.ToListAsync();
        }

        public async Task AddAsync(MedicalRecord record)
        {
            await _db.MedicalRecords.AddAsync(record);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(MedicalRecord record)
        {
            _db.MedicalRecords.Update(record);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _db.MedicalRecords.FindAsync(id);
            if (record != null)
            {
                _db.MedicalRecords.Remove(record);
                await _db.SaveChangesAsync();
            }
        }
    }
}
