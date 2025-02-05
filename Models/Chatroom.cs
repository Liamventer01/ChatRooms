using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatRooms.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Chatroom
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
        public int MsgLengthLimit { get; set; }
        public string? ChatroomImageUrl { get; set; } = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692792846/huhbsufbxyvut0tx0nzy.png";
        [Required]
        public string OwnerId { get; set; }

        public ICollection<User>? Users { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
