using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Repositories
{
    public interface IExaminationImageRepository
    {
        Task<ExaminationImage?> GetByIdAsync(int id);
        Task<List<ExaminationImage>> GetAllAsync();
        Task AddAsync(ExaminationImage image);
        Task UpdateAsync(ExaminationImage image);
        Task DeleteAsync(int id);

        Task<List<ExaminationImage>> GetByExaminationIdAsync(int examinationId);
    }
}
