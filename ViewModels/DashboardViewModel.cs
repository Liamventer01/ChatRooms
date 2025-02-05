using ChatRooms.Helpers;
using ChatRooms.Models;

namespace ChatRooms.ViewModels
{
    public class DashboardViewModel
    {
        public PaginatedList<Chatroom>? OwnedChatrooms { get; set; }
        public PaginatedList<Chatroom>? PinnedChatrooms { get; set; }
        public IQueryable<Chatroom>? PinnedChatroomsQuery { get; set; }
    }
}
