Query 1: Appointments with Doctor and Patient names
SELECT
    a.AppointmentId,
    d.Name AS DoctorName,
    p.Name AS PatientName,
    a.AppointmentDate,
    a.AppointmentTime,
    a.Reason
FROM Appointments a
INNER JOIN Doctors d
    ON a.DoctorId = d.DoctorId
INNER JOIN Patients p
    ON a.PatientId = p.PatientId;


Query 2: Appointment count for each doctor
SELECT
    d.DoctorId,
    d.Name AS DoctorName,
    COUNT(a.AppointmentId) AS AppointmentCount
FROM Doctors d
LEFT JOIN Appointments a
    ON d.DoctorId = a.DoctorId
GROUP BY
    d.DoctorId,
    d.Name
ORDER BY
    AppointmentCount DESC;


Query 3: Doctor with the highest number of appointments
SELECT TOP 1
    d.DoctorId,
    d.Name AS DoctorName,
    COUNT(a.AppointmentId) AS AppointmentCount
FROM Doctors d
LEFT JOIN Appointments a
    ON d.DoctorId = a.DoctorId
GROUP BY
    d.DoctorId,
    d.Name
ORDER BY
    AppointmentCount DESC;


Query 4: Patients with no appointments
SELECT
    p.PatientId,
    p.Name,
    p.Phone,
    p.Email
FROM Patients p
LEFT JOIN Appointments a
    ON p.PatientId = a.PatientId
WHERE a.AppointmentId IS NULL;



Query 5: Duplicate patient phone numbers
SELECT
    Phone,
    COUNT(*) AS DuplicateCount
FROM Patients
WHERE Phone IS NOT NULL
  AND Phone <> ''
GROUP BY Phone
HAVING COUNT(*) > 1;