namespace ChatRooms.Models
{
    public class UserPinnedChatroom
    {
        public string UserId { get; set; }
        public User? User { get; set; }

        public int ChatroomId { get; set; }
        public Chatroom? Chatroom { get; set; }
    }
}
