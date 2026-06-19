
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
            // luego esto pasaria a la capa de APLICACION
            // el controlador recibiria el body, request -> le pasa a la capa de aplicacion, valida, crea al medico, persiste al medico
            // vuelve al controlador, y este simplemente devuelve el created o codigo de estado

            if(string.IsNullOrWhiteSpace(request.name) || 
                string.IsNullOrWhiteSpace(request.LicenseNumber)) 
            {
                throw new ValidationException("Nombre y matricula son requeridas");
            }

            // a fines de responder una validacion que no se supera, es lo mismo badrequest con throw new
            // si yo llevo todo a la capa de app y mantengo return badrequest -> NO VA A FUNCIONAR, no conoce badrequest porque es propio de controller
            // la capa de app no le interesa los codigos de estados ni http. eso es propio de ESTA capa

            // idea: reemplazar las cosas en otras capas y que sigan funcionando. app no va a entregar error.
            //      http solo le interesa a la capa api

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
