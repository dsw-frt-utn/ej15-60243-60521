
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("[api/doctors]")]
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
            if(string.IsNullOrWhiteSpace(request.name) || 
                string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                return BadRequest("Nombre y matricula son requeridas");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId); 
            if (speciality == null)
            {
                return BadRequest("La especialidad no existe");
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
    }
}
