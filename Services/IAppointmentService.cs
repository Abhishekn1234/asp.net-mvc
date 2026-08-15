namespace ClinicAppointmentManagement.Services
{
    public interface IAppointmentService
    {
        Task<bool> IsDoctorAvailableAsync(
            int doctorId,
            DateTime appointmentDate,
            TimeSpan appointmentTime,
            int? appointmentId = null);
    }
}