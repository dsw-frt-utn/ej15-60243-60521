using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data;

public class Dsw2026Ej15DbContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }

    public Dsw2026Ej15DbContext(DbContextOptions<Dsw2026Ej15DbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>(e =>
        {
            e.ToTable("Doctors");
            e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            e.Property(p => p.LicenseNumber).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Speciality>(e =>
        {
            e.ToTable("Specialities");
            e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            e.Property(p => p.Description).HasMaxLength(300).IsRequired();
        });
    }
}
