using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<MedicalRecord?> GetByIdAsync(int id);
        Task<List<MedicalRecord>> GetAllAsync();
        Task AddAsync(MedicalRecord record);
        Task UpdateAsync(MedicalRecord record);
        Task DeleteAsync(int id);

    }
}
