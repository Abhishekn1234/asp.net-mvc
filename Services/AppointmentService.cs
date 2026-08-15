using ClinicAppointmentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDoctorAvailableAsync(
            int doctorId,
            DateTime appointmentDate,
            TimeSpan appointmentTime,
            int? appointmentId = null)
        {
            var query = _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate.Date == appointmentDate.Date &&
                    a.AppointmentTime == appointmentTime);

            
            if (appointmentId.HasValue)
            {
                query = query.Where(
                    a => a.AppointmentId != appointmentId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}