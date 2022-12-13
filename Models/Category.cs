using Microsoft.Build.Framework;

namespace DoctorSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
