using ChatRooms.Data;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRooms.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ChatroomContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ChatroomContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Chatroom>> GetAllUserOwnedChatrooms()
        {
            var currUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var userOwnedChatrooms = _context.Chatrooms.Where(c => c.OwnerId == currUser);
            return await userOwnedChatrooms.ToListAsync();
        }

        public async Task<List<Chatroom>> GetAllUserPinnedChatrooms()
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var userPinnedChatroomIds = await _context.UserPinnedChatrooms
                .Where(upc => upc.UserId == currUserId.ToString())
                .Select(upc => upc.ChatroomId)
                .ToListAsync();

            var pinnedChatrooms = await _context.Chatrooms
                .Where(c => userPinnedChatroomIds.Contains(c.Id))
                .ToListAsync();

            return pinnedChatrooms;
        }

        public IQueryable<Chatroom> GetAllUserOwnedChatroomsQuery()
        {
            var currUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var userOwnedChatrooms = _context.Chatrooms.Where(c => c.OwnerId == currUser);
            return userOwnedChatrooms;
        }

        public IQueryable<Chatroom> GetAllUserPinnedChatroomsQuery()
        {
            var currUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var userPinnedChatroomIds = _context.UserPinnedChatrooms
                .Where(upc => upc.UserId == currUserId.ToString())
                .Select(upc => upc.ChatroomId);

            var pinnedChatrooms = _context.Chatrooms
                .Where(c => userPinnedChatroomIds.Contains(c.Id));

            return pinnedChatrooms;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
