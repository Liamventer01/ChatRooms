using ChatRooms.Models;

namespace ChatRooms.Interfaces
{
    public interface IChatroomRepository
    {
        Task<IEnumerable<Chatroom>> GetAll();
        IQueryable<Chatroom> GetAllQuery();
        Task<Chatroom> GetByIdAsync(int? id);
        Task<Chatroom> GetByIdAsyncNoTracking(int? id);
        Task<Chatroom> GetByNameAsync(string? chatroomName);
        Task<Chatroom> GetByNameAsyncNoTracking(string? chatroomName);

        bool DeleteChatroomsByUserId(string id);
        bool DeletePinnedChatroomsByUserId(string id);

        bool Add(Chatroom chatroom);
        bool Update(Chatroom chatroom);
        bool Delete(Chatroom chatroom);
        bool Save();

    }
}
