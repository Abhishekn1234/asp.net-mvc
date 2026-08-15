using ClinicAppointmentManagement.Data;
using ClinicAppointmentManagement.Models;
using ClinicAppointmentManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentManagement.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(
            ApplicationDbContext context,
            IAppointmentService appointmentService)
        {
            _context = context;
            _appointmentService = appointmentService;
        }

        
        public async Task<IActionResult> Index(
            string? search,
            DateTime? date)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    a.Doctor!.Name.Contains(search) ||
                    a.Patient!.Name.Contains(search));
            }

            if (date.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate.Date ==
                    date.Value.Date);
            }

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Date = date?.ToString("yyyy-MM-dd");

            return View(appointments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(
                    appointment.DoctorId,
                    appointment.PatientId);

                return View(appointment);
            }

            bool available =
                await _appointmentService
                    .IsDoctorAvailableAsync(
                        appointment.DoctorId,
                        appointment.AppointmentDate,
                        appointment.AppointmentTime);

            if (!available)
            {
                ModelState.AddModelError(
                    "",
                    "The doctor is already booked for this date and time.");

                await LoadDropdowns(
                    appointment.DoctorId,
                    appointment.PatientId);

                return View(appointment);
            }

            _context.Appointments.Add(appointment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    "",
                    "The appointment could not be booked. " +
                    "The doctor may already be booked for this time.");

                await LoadDropdowns(
                    appointment.DoctorId,
                    appointment.PatientId);

                return View(appointment);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns(
            int? doctorId = null,
            int? patientId = null)
        {
            var doctors = await _context.Doctors
                .OrderBy(d => d.Name)
                .ToListAsync();

            var patients = await _context.Patients
                .OrderBy(p => p.Name)
                .ToListAsync();

            ViewBag.Doctors = new SelectList(
                doctors,
                "DoctorId",
                "Name",
                doctorId);

            ViewBag.Patients = new SelectList(
                patients,
                "PatientId",
                "Name",
                patientId);
        }
    }
}