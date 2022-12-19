using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }


        public DefaultUser? Creator { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        //!!
        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
