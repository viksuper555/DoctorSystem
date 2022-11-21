using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DoctorSystem.Models
{
    public class RoleRequest
    {
        public int Id { get; set; }

        public DefaultUser? Requester { get; set; }

        public DateTime DateCreated { get; set; }  

        public IdentityRole? Role { get; set; }
        public bool IsApproved { get; set; }

        [NotMapped]
        public string UserId { get; set; }

    }
}
