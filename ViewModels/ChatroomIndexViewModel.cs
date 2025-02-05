using ChatRooms.Helpers;
using ChatRooms.Models;

namespace ChatRooms.ViewModels
{
    public class ChatroomIndexViewModel
    {
        public PaginatedList<Chatroom>? Chatrooms { get; set; }
        public IQueryable<Chatroom>? PinnedChatrooms { get; set; }
    }
}
