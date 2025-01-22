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

        // Patient
        public IPatientRepository CreatePatientRepository()
        {
            return new PatientRepository(_db);
        }

        // Prescription
        public IPrescriptionRepository CreatePrescriptionRepository()
        {
            return new PrescriptionRepository(_db);
        }

        // And so on for other entities...
    }
}
