using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentManagement.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Doctor Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}