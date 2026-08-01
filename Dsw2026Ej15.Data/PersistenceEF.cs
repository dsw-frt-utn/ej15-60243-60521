using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Data;

public class PersistenceEF : IPersistence
{
    private readonly Dsw2026Ej15DbContext _context;

    public PersistenceEF(Dsw2026Ej15DbContext context)
    {
        _context = context;
        LoadSpecialities();
    }

    public async Task AddDoctor(Doctor doctor)
    {
        _context.Add(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctors()
    {
        return await _context.Doctors.Include(d => d.Speciality).Where(d => d.IsActive).ToListAsync();
    }

    public async Task<Doctor?> GetDoctor(Guid doctorId)
    {
        return await _context.Doctors.Include(d => d.Speciality).FirstOrDefaultAsync(d => d.Id == doctorId && d.IsActive);
    }

    public async Task<Speciality?> GetSpecialityById(Guid id)
    {
        return await _context.Specialities.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateDoctor(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
    }

    private void LoadSpecialities()
    {
        try
        {
            if (!_context.Specialities.Any())
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");
                var json = File.ReadAllText(jsonPath);
                var specialitiesDto = JsonSerializer.Deserialize<List<SpecialityDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

                var specialities = specialitiesDto.Select(s => new Speciality(s.Name, s.Description, s.Id)).ToList();

                _context.Specialities.AddRange(specialities);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}