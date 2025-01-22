using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApp.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB must be 11 characters.")]
        public string OIB { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Examination> Examinations { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }

        public Patient()
        {
            MedicalRecords = new List<MedicalRecord>();
            Examinations = new List<Examination>();
            Prescriptions = new List<Prescription>();
        }
    }
}
