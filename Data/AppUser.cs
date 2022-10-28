using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Data
{
    public class AppUser : IdentityUser
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string gender {get; set;}

        [DataType(DataType.Date)]
        public string dateOfBirth {get; set;}
    }
}
