namespace ChatRooms.Interfaces
{
    public interface IChatroomService
    {
        Task PinChatroomAsync(string userId, int chatroomId);
        Task UnpinChatroomAsync(string userId, int chatroomId);
    }
}
