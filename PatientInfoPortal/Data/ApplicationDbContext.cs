using Microsoft.EntityFrameworkCore;
using PatientInfoPortal.Models;

namespace PatientInfoPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientNCD> PatientNCDs { get; set; }
        public DbSet<PatientAllergy> PatientAllergies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PatientNCD>()
                .HasOne(pn => pn.Patient)
                .WithMany(p => p.PatientNCDs)
                .HasForeignKey(pn => pn.PatientId);

            modelBuilder.Entity<PatientAllergy>()
                .HasOne(pa => pa.Patient)
                .WithMany(p => p.PatientAllergies)
                .HasForeignKey(pa => pa.PatientId);
        }
    }
}
