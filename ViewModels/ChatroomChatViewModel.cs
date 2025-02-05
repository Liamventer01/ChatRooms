using ChatRooms.Models;

namespace ChatRooms.ViewModels
{
    public class ChatroomChatViewModel
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int? ChatroomId { get; set; }
        public string? ChatroomName { get; set; }
        public int? MsgLengthLimit { get; set; }
        public string? MessageContent { get; set; }

        public IEnumerable<Message>? Messages { get; set; }
    }
}
