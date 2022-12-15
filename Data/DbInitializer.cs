using Microsoft.AspNetCore.Identity;
using DoctorSystem.Misc;

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
                Name = Role.Admin,
                NormalizedName = Role.Admin.ToUpper(),
            });

            context.Roles.Add(new IdentityRole
            {
                Name = Role.Guest,
                NormalizedName = Role.Guest.ToUpper(),
            });

            context.Roles.Add(new IdentityRole
            {
                Name = Role.Patient,
                NormalizedName = Role.Patient.ToUpper(),
            });

            context.Roles.Add(new IdentityRole
            {
                Name = Role.Doctor,
                NormalizedName = Role.Doctor.ToUpper(),
            });
            context.SaveChanges();
        }
    }
}