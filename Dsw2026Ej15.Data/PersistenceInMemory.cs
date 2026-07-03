using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private List<Speciality> _specialities = [];
    private List<Doctor> _doctors = [];

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    public async Task AddDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctors()
    {
        return _doctors;
    }

    public async Task<Doctor?> GetDoctor(Guid doctorId)
    {
        return _doctors.FirstOrDefault(d => d.Id == doctorId);
    }

    public async Task<Speciality?> GetSpecialityById(Guid id)
    {
        return _specialities.SingleOrDefault(s => s.Id == id);
    }

    public async Task UpdateDoctor(Doctor doctor)
    {
        _doctors.Remove(doctor);
        _doctors.Add(doctor);
    }

    private void LoadSpecialities()
    {
        try
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");
            var json = File.ReadAllText(jsonPath);
            var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
            _specialities = [.. specialities.Select(s => new Speciality(s.Name, s.Description, s.Id))];
        }
        catch (Exception ex)
        {
        }
    }
}
