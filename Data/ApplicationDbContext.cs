using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.firstName)
                .HasMaxLength(250);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.lastName)
                .HasMaxLength(250);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.gender)
                .HasMaxLength(250);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.dateOfBirth)
                .HasMaxLength(250);

        }
    }
}