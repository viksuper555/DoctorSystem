using Microsoft.AspNetCore.Identity;

namespace DoctorSystem.Data;
public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();


        if (!context.Roles.Any())
        {
            context.Roles.Add(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            context.Roles.Add(new IdentityRole
            {
                Name = "Guest",
                NormalizedName = "GUEST"
            });

            context.Roles.Add(new IdentityRole
            {
                Name = "Patient",
                NormalizedName = "PATIENT"
            });

            context.Roles.Add(new IdentityRole
            {
                Name = "Doctor",
                NormalizedName = "DOCTOR"
            });
            context.SaveChanges();
        }
    }
}