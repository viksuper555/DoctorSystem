namespace DoctorSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public DefaultUser Creator { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Text { get; set; }

        public Post Post { get; set; }
    }
}
