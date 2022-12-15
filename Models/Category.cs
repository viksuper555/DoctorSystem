using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoctorSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
