using Humanizer;
using MessagePack;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Models
{
    public class DefaultUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }
        
        public string FullName
        {
            get {
                if (DoctorUID != null) { return "Dr." + FirstName + " " + LastName; }
                return FirstName + " " + LastName; 
            }
        }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? DoctorUID { get; set; }
        public ICollection<UserFriendship> FriendsOf { get; set; }
        public ICollection<UserFriendship> Friends { get; set; }
       
        
    }

    public class UserFriendship
    {
        public string UserId { get; set; }
        public DefaultUser User { get; set; }

        public string UserFriendId { get; set; }
        public DefaultUser UserFriend { get; set; }
    }
}
