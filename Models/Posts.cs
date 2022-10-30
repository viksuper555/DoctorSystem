using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Models
{
    public class Posts
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateOnly DateCreated { get; set; }

        //така или свързочна таблица(OneToMany)
        public IEnumerable<string> Comments { get; set; }



    }
}
