using Microsoft.AspNetCore.Identity;

namespace ChatRooms.Models
{
    public class User : IdentityUser
    {
        public string? ProfileImageUrl { get; set; } = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692785238/h1jgoxervta83v5xpljo.png";
        public ICollection<Message>? Messages { get; set; }
        public ICollection<Chatroom>? Chatrooms { get; set; }
        public ICollection<UserPinnedChatroom>? PinnedChatrooms { get; set; }
    }
}
