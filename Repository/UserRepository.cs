using ChatRooms.Data;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRooms.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatroomContext _context;

        public UserRepository(ChatroomContext context)
        {
            _context = context;
        }
        public bool Add(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Delete(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string? id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByIdAsyncNoTracking(string? id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByNameAsync(string? userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User> GetUserByNameAsyncNoTracking(string? userName)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName);
        }


        public int CountMessagesByUserId(string id)
        {
            var userMessages = _context.Messages.Where(m => m.UserId == id);
            return userMessages.Count();
        }

        public int CountChatroomsByUserId(string id)
        {
            var userChatrooms = _context.Chatrooms.Where(c => c.OwnerId == id);
            return userChatrooms.Count();
        }

        public int CountPinnedChatroomsByUserId(string id)
        {
            var userPinnedChatrooms = _context.UserPinnedChatrooms.Where(upc => upc.UserId == id);
            return userPinnedChatrooms.Count();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
