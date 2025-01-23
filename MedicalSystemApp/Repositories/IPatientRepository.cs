using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(int id);
        Task<List<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);

        Task<List<Patient>> SearchAsync(string searchTerm);
    }
}
