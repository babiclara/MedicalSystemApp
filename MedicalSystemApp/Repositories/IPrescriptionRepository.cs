using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<Prescription?> GetByIdAsync(int id);
        Task<List<Prescription>> GetAllAsync();
        Task AddAsync(Prescription prescription);
        Task UpdateAsync(Prescription prescription);
        Task DeleteAsync(int id);

    }
}
