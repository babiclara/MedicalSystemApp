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

        // If you want advanced queries (like searching by IllnessName), add them here
        // Task<List<MedicalRecord>> SearchByIllnessName(string illness);
    }
}
