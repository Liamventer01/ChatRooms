using ChatRooms.Data;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ChatRooms.Services
{
    public class ChatroomService : IChatroomService
    {
        private readonly IChatroomRepository _chatroomRepository;
        private readonly IUserRepository _userRepository;
        private readonly ChatroomContext _context;

        public ChatroomService(IChatroomRepository chatroomRepository, IUserRepository userRepository, ChatroomContext context)
        {
            _chatroomRepository = chatroomRepository;
            _userRepository = userRepository;
            _context = context;
        }
        public async Task PinChatroomAsync(string userId, int chatroomId)
        {
            var userPinnedChatroom = new UserPinnedChatroom
            {
                UserId = userId,
                ChatroomId = chatroomId,
            };

            try
            {
                _context.UserPinnedChatrooms.Add(userPinnedChatroom);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Logging runtime exception
                Log.Error("Unhandled runtime exception occurred", ex);
            }
        }

        public async Task UnpinChatroomAsync(string userId, int chatroomId)
        {
            var userPinnedChatroom = await _context.UserPinnedChatrooms
                .SingleOrDefaultAsync(upc => upc.UserId == userId && upc.ChatroomId == chatroomId);

            if (userPinnedChatroom != null)
            {
                _context.UserPinnedChatrooms.Remove(userPinnedChatroom);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Logging runtime exception
                Log.Error("PinnedChatroom is null");
            }
        }
    }
}
