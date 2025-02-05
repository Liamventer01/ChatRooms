using ChatRooms.Data;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRooms.Repository
{
    public class ChatroomRepository : IChatroomRepository
    {
        private readonly ChatroomContext _context;

        public ChatroomRepository(ChatroomContext context)
        {
            _context = context;
        }
        public bool Add(Chatroom chatroom)
        {
            _context.Add(chatroom);
            return Save();
        }

        public bool Delete(Chatroom chatroom)
        {
            _context.Remove(chatroom);
            return Save();
        }

        public async Task<IEnumerable<Chatroom>> GetAll()
        {
            return await _context.Chatrooms.ToListAsync();
        }

        public IQueryable<Chatroom> GetAllQuery()
        {
            var chatrooms = from c in _context.Chatrooms
                            select c;
            return chatrooms;
        }

        public async Task<Chatroom> GetByIdAsync(int? id)
        {
            return await _context.Chatrooms.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Chatroom> GetByIdAsyncNoTracking(int? id)
        {
            return await _context.Chatrooms.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Chatroom> GetByNameAsync(string? chatroomName)
        {
            return await _context.Chatrooms.FirstOrDefaultAsync(i => i.Name == chatroomName);
        }

        public async Task<Chatroom> GetByNameAsyncNoTracking(string? chatroomName)
        {
            return await _context.Chatrooms.AsNoTracking().FirstOrDefaultAsync(i => i.Name == chatroomName);
        }

        public bool DeleteChatroomsByUserId(string id)
        {
            var userChatrooms = _context.Chatrooms.Where(c => c.OwnerId == id);
            _context.Chatrooms.RemoveRange(userChatrooms);
            return Save();
        }

        public bool DeletePinnedChatroomsByUserId(string id)
        {
            var userPinnedChatrooms = _context.UserPinnedChatrooms.Where(upc => upc.UserId == id);
            _context.UserPinnedChatrooms.RemoveRange(userPinnedChatrooms);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Chatroom chatroom)
        {
            _context.Update(chatroom);
            return Save();
        }
    }
}
