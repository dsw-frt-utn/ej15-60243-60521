using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Speciality? GetSpecialityById(Guid id);

        List<Doctor> GetAllActiveDoctors();
        Doctor? GetActiveDoctorById(Guid id);
        void AddDoctor(Doctor doctor);
        void DeleteDoctor(Doctor doctor);
    }
}
