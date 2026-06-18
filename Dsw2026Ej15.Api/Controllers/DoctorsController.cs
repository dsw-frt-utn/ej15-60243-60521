
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;


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
        public IActionResult CreateDoctor([FromBody] DoctorModel.Request request)
        {
            if(string.IsNullOrWhiteSpace(request.name) || 
                string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Nombre y matricula son requeridas");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId); 
            if (speciality == null)
            {
                throw new ValidationException("La especialidad no existe");
            }
            var newDoctor = new Doctor
            {
                Id = Guid.NewGuid(), 
                Name = request.name,
                LicenseNumber = request.LicenseNumber,
                SpecialityId = request.SpecialityId,
                IsActive = true 
            };

            _persistence.AddDoctor(newDoctor);

            return Created(); // Created=201
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = _persistence.GetDoctors()
                .Where(d => d.IsActive)
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.LicenseNumber,
                    d.SpecialityId
                });

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public IActionResult GetDoctorById(Guid id)
        {
            var doctor = _persistence.GetDoctorById(id);

            if (doctor == null || !doctor.IsActive)
            {
                return NotFound();
            }

            var speciality = _persistence.GetSpecialityById(doctor.SpecialityId);

            var response = new
            {
                doctor.Name,
                doctor.LicenseNumber,
                SpecialityName = speciality?.Name
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(Guid id)
        {
            var doctor = _persistence.GetDoctorById(id);

            if (doctor == null || !doctor.IsActive)
            {
                return NotFound();
            }

            _persistence.DeleteDoctor(id);

            return NoContent();
        }
    }

}
