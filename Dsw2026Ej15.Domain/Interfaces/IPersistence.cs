using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Speciality? GetSpecialityById(Guid id);
        IEnumerable<Speciality> GetSpecialities();
        List<Doctor> GetAllActiveDoctors();
        IEnumerable<Doctor> GetDoctors();

        Doctor? GetDoctorById(Guid id);
        void AddDoctor(Doctor doctor);
        void DeleteDoctor(Guid id);
    }
}
