using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
   
    private List<Speciality> _specialities = new();
    private readonly List<Doctor> _doctors = new();

    public PersistenceInMemory()
    {
        
        LoadSpecialities();
    }

    private void LoadSpecialities()
    {
        try
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");
            var json = File.ReadAllText(jsonPath);

            var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

            _specialities = specialities.Select(s => new Speciality
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cargando especialidades: {ex.Message}");
        }
    }

    public Speciality? GetSpecialityById(Guid id)
    {
        return _specialities.SingleOrDefault(e => e.Id == id);
    }
    public IEnumerable<Speciality> GetSpecialities()
    {
        return _specialities;
    }

    public List<Doctor> GetAllActiveDoctors()
    {
        return _doctors.Where(d => d.IsActive).ToList();
    }
    public IEnumerable<Doctor> GetDoctors()
    {
        return _doctors;
    }

    public Doctor? GetDoctorById(Guid id)
    {
        return _doctors.FirstOrDefault(d => d.Id == id && d.IsActive);
    }

    public void AddDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    public void DeleteDoctor(Guid id)
    {
        var doctorInDb = _doctors.FirstOrDefault(d => d.Id == id);

        if (doctorInDb != null)
        {
            doctorInDb.IsActive = false;
        }
    }
}
