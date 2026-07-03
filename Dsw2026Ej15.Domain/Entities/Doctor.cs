using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        private Speciality speciality;
        public string Name { get; init; }
        public string? Email { get; private set; }
        public string LicenseNumber { get; init; }
        public bool IsActive { get; private set; }
        public Guid? SpecialityId { get; set; }
        public Speciality? Speciality { get; private set; }

        private Doctor()
        {
        }

        public Doctor(string name, string licenseNumber, Speciality? speciality, Guid? id = null) : base(id)
        {
            Name = name;
            LicenseNumber = licenseNumber;
            IsActive = true;
            Speciality = speciality;
        }

        public void Deactive()
        {
            IsActive = false;
        }
    }

}
