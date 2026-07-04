using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dsw2026Ej15.Data;

public class PersistenceEf : IPersistence
{
    private readonly Ej16DbContext _context;

    public PersistenceEf(Ej16DbContext context)
    {
        _context = context;
        LoadSpecialities();
    }

    public Speciality? GetSpecialityById(Guid id)
    {
        return _context.Specialities.SingleOrDefault(s => s.Id == id);
    }

    public IEnumerable<Speciality> GetSpecialities()
    {
        return _context.Specialities.AsNoTracking().ToList();
    }

    public List<Doctor> GetAllActiveDoctors()
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .Where(d => d.IsActive)
            .ToList();
    }

    public IEnumerable<Doctor> GetDoctors()
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .ToList();
    }

    public Doctor? GetDoctorById(Guid id)
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .FirstOrDefault(d => d.Id == id && d.IsActive);
    }

    public void AddDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        _context.SaveChanges();
    }

    public void DeleteDoctor(Guid id)
    {
        var doctorInDb = _context.Doctors
            .FirstOrDefault(d => d.Id == id && d.IsActive);

        if (doctorInDb != null)
        {
            doctorInDb.IsActive = false;
            _context.SaveChanges();
        }
    }

    private void LoadSpecialities()
    {
        if (_context.Specialities.Any())
        {
            return;
        }

        string jsonPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Sources",
            "specialities.json"
        );

        var json = File.ReadAllText(jsonPath);

        var specialitiesDto = JsonSerializer.Deserialize<List<SpecialityDto>>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? [];

        var specialities = specialitiesDto.Select(s => new Speciality
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description
        }).ToList();

        _context.Specialities.AddRange(specialities);
        _context.SaveChanges();
    }
}
