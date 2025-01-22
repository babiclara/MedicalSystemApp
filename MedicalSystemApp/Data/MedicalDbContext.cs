using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Models;

namespace MedicalSystemApp.Data
{
    public class MedicalDbContext : DbContext
    {
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<ExaminationImage> ExaminationImages { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.OIB)
                .IsUnique();

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicalRecord>()
                .Property(m => m.StartDate)
                .HasColumnType("date");

            modelBuilder.Entity<MedicalRecord>()
                .Property(m => m.EndDate)
                .HasColumnType("date");

            modelBuilder.Entity<Examination>()
                .HasOne(e => e.Patient)
                .WithMany(p => p.Examinations)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Examination>()
                .Property(e => e.ExaminationDateTime)
                .HasColumnType("timestamp without time zone")
                .HasConversion(
                    v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified),
                    v => v
                );

            modelBuilder.Entity<ExaminationImage>()
                .HasOne(img => img.Examination)
                .WithMany(e => e.ExaminationImages)
                .HasForeignKey(img => img.ExaminationId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Prescription>()
                .HasOne(pr => pr.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prescription>()
                .Property(p => p.DatePrescribed)
                .HasColumnType("date");
        }
    }
}
