using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApp.Models
{
    public class ExaminationImage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string ImagePath { get; set; }

        public int ExaminationId { get; set; }

        [ForeignKey("ExaminationId")]
        public virtual Examination? Examination { get; set; }
    }
}
