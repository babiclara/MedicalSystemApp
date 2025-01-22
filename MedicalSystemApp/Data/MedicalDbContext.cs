using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models; // Adjust if your Models namespace is different

namespace MedicalSystemApp.Data
{
    public class MedicalDbContext : DbContext
    {
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options)
            : base(options)
        {
        }

        // DbSets for each of your models
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<ExaminationImage> ExaminationImages { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //
            // Example: Make OIB unique for each Patient
            //
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.OIB)
                .IsUnique();

            //
            // Configure relationships explicitly (if you want more control)
            // EF can infer these from your foreign key properties, but this is more explicit.
            //

            // 1) Patient -> MedicalRecords (1:N)
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2) Patient -> Examinations (1:N)
            modelBuilder.Entity<Examination>()
                .HasOne(e => e.Patient)
                .WithMany(p => p.Examinations)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3) Examination -> ExaminationImages (1:N)
            modelBuilder.Entity<ExaminationImage>()
                .HasOne(img => img.Examination)
                .WithMany(e => e.ExaminationImages)
                .HasForeignKey(img => img.ExaminationId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4) Patient -> Prescriptions (1:N)
            modelBuilder.Entity<Prescription>()
                .HasOne(pr => pr.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add other Fluent configurations as needed...
        }
    }
}
