using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Domain.Interfaces;

public interface IPersistence
{
    Task AddDoctor(Doctor doctor);
    Task<Speciality?> GetSpecialityById(Guid id);
    Task<IEnumerable<Doctor>> GetAllDoctors();
    Task<Doctor?> GetDoctor(Guid doctorId);
    Task UpdateDoctor(Doctor doctor);
}