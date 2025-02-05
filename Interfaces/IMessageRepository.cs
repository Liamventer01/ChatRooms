using ChatRooms.Models;

namespace ChatRooms.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAll();
        Task<Message> GetByIdAsync(int? id);
        Task<Message> GetByIdAsyncNoTracking(int? id);
        Task<IEnumerable<Message>> GetMessagesByChatroomId(int? id);
        Task<IEnumerable<Message>> GetMessagesByChatroomIdTake(int? id, int amount);

        bool DeleteMessagesByUserId(string id);

        bool Add(Message message);
        bool Update(Message message);
        bool Delete(Message message);
        bool Save();
    }
}
