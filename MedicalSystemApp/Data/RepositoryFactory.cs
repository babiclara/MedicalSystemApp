using MedicalSystemApp.Data;

namespace MedicalSystemApp.Repositories
{
    public class RepositoryFactory
    {
        private readonly MedicalDbContext _db;

        public RepositoryFactory(MedicalDbContext db)
        {
            _db = db;
        }

        public IPatientRepository CreatePatientRepository()
        {
            return new PatientRepository(_db);
        }

        public IPrescriptionRepository CreatePrescriptionRepository()
        {
            return new PrescriptionRepository(_db);
        }

        public IMedicalRecordRepository CreateMedicalRecordRepository()
        {
            return new MedicalRecordRepository(_db);
        }

        public IExaminationImageRepository CreateExaminationImageRepository()
        {
            return new ExaminationImageRepository(_db);
        }

        public IExaminationRepository CreateExaminationRepository()
        {
            return new ExaminationRepository(_db);
        }
    }
}
