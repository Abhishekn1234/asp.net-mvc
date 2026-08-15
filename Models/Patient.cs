using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentManagement.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Patient Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}