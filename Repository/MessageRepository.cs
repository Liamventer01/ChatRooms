using ChatRooms.Data;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRooms.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatroomContext _context;
        public MessageRepository(ChatroomContext context)
        {
            _context = context;
        }
        public bool Add(Message message)
        {
            _context.Add(message);
            return Save();
        }

        public bool Delete(Message message)
        {
            _context.Remove(message);
            return Save();
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message> GetByIdAsync(int? id)
        {
            return await _context.Messages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Message> GetByIdAsyncNoTracking(int? id)
        {
            return await _context.Messages.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatroomId(int? id)
        {
            return await _context.Messages
                .Include(m => m.User)
                .Where(message => message.ChatroomId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatroomIdTake(int? id, int amount)
        {
            return await _context.Messages
                .Include(m => m.User)
                .Where(message => message.ChatroomId == id)
                .Take(amount).ToListAsync();
        }

        public bool DeleteMessagesByUserId(string id)
        {
            var userMessages = _context.Messages.Where(m => m.UserId == id);
            _context.Messages.RemoveRange(userMessages);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Message message)
        {
            _context.Update(message);
            return Save();
        }
    }
}
