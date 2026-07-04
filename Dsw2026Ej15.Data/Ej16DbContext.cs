using Dsw2026Ej15.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data
{
    public class Ej16DbContext : DbContext
    {
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Speciality> Specialities => Set<Speciality>();

        public Ej16DbContext(DbContextOptions<Ej16DbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(d => d.LicenseNumber)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(d => d.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Speciality)
                    .WithMany()
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.ToTable("Specialities");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(s => s.Description)
                    .HasMaxLength(300)
                    .IsRequired();
            });
        }
    }
}
