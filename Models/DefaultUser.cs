using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Models
{
    public class DefaultUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? DoctorUID { get; set; }
    }
}
