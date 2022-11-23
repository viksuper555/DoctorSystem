using DoctorSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<DefaultUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<RoleRequest> RoleRequests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DefaultUser>()
                .Property(e => e.FirstName)
                .HasMaxLength(250);

            modelBuilder.Entity<DefaultUser>()
                .Property(e => e.LastName)
                .HasMaxLength(250);

            modelBuilder.Entity<DefaultUser>()
                .Property(e => e.Gender)
                .HasMaxLength(250);

            modelBuilder.Entity<DefaultUser>()
                .Property(e => e.DateOfBirth);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.Post)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}