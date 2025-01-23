using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public interface IExaminationRepository
    {
        Task<Examination?> GetByIdAsync(int id);
        Task<List<Examination>> GetAllAsync();
        Task AddAsync(Examination exam);
        Task UpdateAsync(Examination exam);
        Task DeleteAsync(int id);

        Task<List<Examination>> GetByTypeAsync(string examType);

        Task<List<Examination>> GetByPatientIdAsync(int patientId);
    }
}
