
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.name) ||
                string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Nombre y matricula son requeridas");
            }

            // Agregamos el await
            var speciality = await _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality == null)
            {
                throw new ValidationException("La especialidad no existe");
            }

            // Adaptado al nuevo constructor de tu compañero
            var newDoctor = new Doctor(request.name, request.LicenseNumber, speciality);

            // Agregamos el await
            await _persistence.AddDoctor(newDoctor);

            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            // El método ahora se llama GetAllDoctors y ya filtra por IsActive internamente[cite: 1]
            var doctors = await _persistence.GetAllDoctors();

            var response = doctors.Select(d => new
            {
                d.Id,
                d.Name,
                d.LicenseNumber,
                SpecialityId = d.Speciality?.Id // Accedemos a la especialidad anidada
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            // El método ahora se llama GetDoctor[cite: 1]
            var doctor = await _persistence.GetDoctor(id);

            // Ya no hace falta validar IsActive acá porque el método GetDoctor de tu compa ya lo filtra[cite: 1]
            if (doctor == null)
            {
                return NotFound();
            }

            var response = new
            {
                doctor.Name,
                doctor.LicenseNumber,
                SpecialityName = doctor.Speciality?.Name // EF incluye la especialidad automáticamente[cite: 1]
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var doctor = await _persistence.GetDoctor(id);

            if (doctor == null)
            {
                return NotFound();
            }

            // Adaptado a la baja lógica de tu compañero[cite: 1]
            doctor.Deactive();
            await _persistence.UpdateDoctor(doctor);

            return NoContent();
        }
    }
}
