using ChatRooms.Helpers;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Security.Claims;

namespace ChatRooms.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatroomRepository _chatroomRepository;
        private readonly IUserRepository _userRepository;

        public ChatHub(IMessageRepository messageRepository, IChatroomRepository chatroomRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _chatroomRepository = chatroomRepository;
            _userRepository = userRepository;
        }
        public async Task JoinRoom(string chatroomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatroomName);

            // Find the claim associated with the user ID
            var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;
                var user = await _userRepository.GetUserByIdAsync(userId);

                if (user != null)
                {
                    string userImageUrl = user.ProfileImageUrl;
                    string userName = user.UserName;
                    string joinMessage = $"has joined the {chatroomName} Chat";
                    string timeStamp = FormatTime.FormatTimeStamp(DateTime.Now, DateTime.Now);

                    await Clients.Group(chatroomName).SendAsync("ReceiveSystemMessage", userImageUrl, userName, joinMessage, timeStamp);

                    // Logging message to maintain log of conversation history
                    Log.Information($"{userImageUrl} {userName} {joinMessage} {timeStamp}");
                }
                else
                {
                    // Logging runtime exception
                    Log.Error("User is null");
                }
            }
            else
            {
                // Logging runtime exception
                Log.Error("User is null");
            }
        }

        public async Task LeaveRoom(string chatroomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatroomName);

            // Find the claim associated with the user ID
            var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;
                var user = await _userRepository.GetUserByIdAsync(userId);

                if (user != null)
                {
                    string userImageUrl = user.ProfileImageUrl;
                    string userName = user.UserName;
                    string leaveMessage = $"has left the {chatroomName} Chat";
                    string timeStamp = FormatTime.FormatTimeStamp(DateTime.Now, DateTime.Now);

                    await Clients.Group(chatroomName).SendAsync("ReceiveSystemMessage", userImageUrl, userName, leaveMessage, timeStamp);

                    // Logging message to maintain log of conversation history
                    Log.Information($"{userImageUrl} {userName} {leaveMessage} {timeStamp}");
                }
                else
                {
                    // Logging runtime exception
                    Log.Error("User is null");
                }
            }
            else
            {
                // Logging runtime exception
                Log.Error("User is null");
            }
        }


        public async Task SendMessageToGroup(string chatroomName, string userId, string messageContent)
        {
            var chatroom = await _chatroomRepository.GetByNameAsync(chatroomName);
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (chatroom != null && user != null)
            {
                var newMessage = new Message
                {
                    Content = messageContent,
                    Length = messageContent.Length,
                    TimeStamp = DateTime.Now,
                    UserId = userId,
                    ChatroomId = chatroom.Id,
                    User = user,
                };

                _messageRepository.Add(newMessage);


                string messageTimeStamp = FormatTime.FormatTimeStamp(newMessage.TimeStamp, DateTime.Now);
                await Clients.Group(chatroomName).SendAsync("ReceiveMessage", user.ProfileImageUrl, user.UserName, messageTimeStamp, newMessage.Content);

                // Logging message to maintain log of conversation history
                Log.Information($"{user.ProfileImageUrl} {user.UserName} {messageTimeStamp} {newMessage.Content}");
            }
            else
            {
                // Logging runtime exception
                Log.Error("User or Chatroom is null");
            }
        }
    }
}
