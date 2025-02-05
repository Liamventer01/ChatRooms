using ChatRooms.Helpers;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using ChatRooms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ChatRooms.Controllers
{
    public class ChatroomsController : Controller
    {
        private readonly IChatroomRepository _chatroomRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IChatroomService _chatroomService;
        private readonly IDashboardRepository _dashboardRepository;

        public ChatroomsController(IChatroomRepository chatroomRepository, IMessageRepository messageRepository, IUserRepository userRepository, IPhotoService photoService, IChatroomService chatroomService, IDashboardRepository dashboardRepository)
        {
            _chatroomRepository = chatroomRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _photoService = photoService;
            _chatroomService = chatroomService;
            _dashboardRepository = dashboardRepository;
        }

        // GET: Chatrooms
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var chatrooms = _chatroomRepository.GetAllQuery();

            if (!String.IsNullOrEmpty(searchString))
            {
                chatrooms = chatrooms.Where(c => c.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    chatrooms = chatrooms.OrderByDescending(c => c.Name);
                    break;
                default:
                    chatrooms = chatrooms.OrderBy(c => c.Name);
                    break;
            }

            int pageSize = 6;

            var pinnedChatrooms = _dashboardRepository.GetAllUserPinnedChatroomsQuery();
            var chatroomVM = new ChatroomIndexViewModel()
            {
                Chatrooms = await PaginatedList<Chatroom>.CreateAsync(chatrooms.AsNoTracking(), pageNumber ?? 1, pageSize),
                PinnedChatrooms = pinnedChatrooms,

            };
            return View(chatroomVM);
        }

        // GET: Chatrooms/Details/1
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {

            Chatroom chatroom = await _chatroomRepository.GetByIdAsync(id);
            var ownerId = chatroom.OwnerId;
            var user = await _userRepository.GetUserByIdAsync(ownerId);
            var chatroomDetailsVM = new ChatroomDetailsViewModel
            {
                Id = id,
                Name = chatroom.Name,
                Description = chatroom.Description,
                MsgLengthLimit = chatroom.MsgLengthLimit,
                ChatroomImageUrl = chatroom.ChatroomImageUrl,
                OwnerUserName = user.UserName,
                OwnerId = ownerId,
            };
            return View(chatroomDetailsVM);
        }

        // GET: Chatrooms/Chat/1
        [Authorize]
        public async Task<IActionResult> Chat(int id)
        {
            var messages = await _messageRepository.GetMessagesByChatroomIdTake(id, 25);
            var chatroom = await _chatroomRepository.GetByIdAsync(id);
            var userId = HttpContext.User.GetUserId();
            var user = await _userRepository.GetUserByIdAsync(userId);
            var chatViewModel = new ChatroomChatViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
                ChatroomId = id,
                ChatroomName = chatroom.Name,
                MsgLengthLimit = chatroom.MsgLengthLimit,
                Messages = messages,
            };

            return View(chatViewModel);
        }

        // GET: Chatrooms/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var currUserId = HttpContext.User.GetUserId();
            var createChatroomVM = new ChatroomCreateViewModel { OwnerId = currUserId };
            return View(createChatroomVM);
        }

        // POST: Chatrooms/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChatroomCreateViewModel chatroomVM)
        {
            if (ModelState.IsValid)
            {
                if (chatroomVM.Image == null)
                {
                    var chatroom = new Chatroom
                    {
                        Name = chatroomVM.Name,
                        Description = chatroomVM.Description,
                        MsgLengthLimit = chatroomVM.MsgLengthLimit,
                        ChatroomImageUrl = chatroomVM.ChatroomImageUrl,
                        OwnerId = chatroomVM.OwnerId,
                    };
                    _chatroomRepository.Add(chatroom);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    var result = await _photoService.AddThumbnailAsync(chatroomVM.Image);

                    var chatroom = new Chatroom
                    {
                        Name = chatroomVM.Name,
                        Description = chatroomVM.Description,
                        MsgLengthLimit = chatroomVM.MsgLengthLimit,
                        ChatroomImageUrl = result.Url.ToString(),
                        OwnerId = chatroomVM.OwnerId,
                    };
                    _chatroomRepository.Add(chatroom);
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            // Logging runtime exception
            Log.Error("Unhandled exception occurred");
            return View(chatroomVM);
        }

        // GET: Chatrooms/Edit/1
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var chatroom = await _chatroomRepository.GetByIdAsync(id);
            if (chatroom == null)
            {
                // Logging runtime exception
                Log.Error("Chatroom is null");
                return View("Error");
            }
            var ownerId = chatroom.OwnerId;

            if (HttpContext.User.GetUserId() != ownerId)
            {
                return RedirectToAction("Index");
            }

            var user = await _userRepository.GetUserByIdAsync(ownerId);
            var chatroomDetailsVM = new ChatroomDetailsViewModel
            {
                Name = chatroom.Name,
                Description = chatroom.Description,
                MsgLengthLimit = chatroom.MsgLengthLimit,
                ChatroomImageUrl = chatroom.ChatroomImageUrl,
                OwnerUserName = user.UserName,
                OwnerId = ownerId,
            };
            return View(chatroomDetailsVM);
        }

        // POST: Chatrooms/Edit/1
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChatroomDetailsViewModel chatRoomDetailsVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit chatroom");
                return View("Edit", chatRoomDetailsVM);
            }

            var userChatroom = await _chatroomRepository.GetByIdAsyncNoTracking(id);

            if (userChatroom == null)
            {
                // Logging runtime exception
                Log.Error("Chatroom is null");
                return View("Error");
            }

            if (chatRoomDetailsVM.Image == null)
            {
                var chatroom = new Chatroom
                {
                    Id = id,
                    Name = chatRoomDetailsVM.Name,
                    Description = chatRoomDetailsVM.Description,
                    MsgLengthLimit = chatRoomDetailsVM.MsgLengthLimit,
                    ChatroomImageUrl = userChatroom.ChatroomImageUrl,
                    OwnerId = chatRoomDetailsVM.OwnerId,
                };

                _chatroomRepository.Update(chatroom);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                var photoResult = await _photoService.AddThumbnailAsync(chatRoomDetailsVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(chatRoomDetailsVM);
                }

                if (!string.IsNullOrEmpty(userChatroom.ChatroomImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(userChatroom.ChatroomImageUrl);
                }

                var chatroom = new Chatroom
                {
                    Id = id,
                    Name = chatRoomDetailsVM.Name,
                    Description = chatRoomDetailsVM.Description,
                    MsgLengthLimit = chatRoomDetailsVM.MsgLengthLimit,
                    ChatroomImageUrl = photoResult.Url.ToString(),
                    OwnerId = chatRoomDetailsVM.OwnerId,
                };

                _chatroomRepository.Update(chatroom);

                return RedirectToAction("Index", "Dashboard");
            }
        }

        // GET: Chatrooms/Delete/1
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var chatroom = await _chatroomRepository.GetByIdAsync(id);
            if (chatroom == null)
            {
                // Logging runtime exception
                Log.Error("Chatroom is null");
                return View("Error");
            }

            var ownerId = chatroom.OwnerId;

            if (HttpContext.User.GetUserId() != ownerId)
            {
                return RedirectToAction("Index");
            }

            var user = await _userRepository.GetUserByIdAsync(ownerId);
            var chatroomDetailsVM = new ChatroomDetailsViewModel
            {
                Name = chatroom.Name,
                Description = chatroom.Description,
                MsgLengthLimit = chatroom.MsgLengthLimit,
                ChatroomImageUrl = chatroom.ChatroomImageUrl,
                OwnerUserName = user.UserName,
                OwnerId = ownerId,
            };
            return View(chatroomDetailsVM);
        }

        // POST: Chatrooms/Delete/1
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatRoomDetails = await _chatroomRepository.GetByIdAsync(id);
            if (chatRoomDetails == null)
            {
                // Logging runtime exception
                Log.Error("Chatroom is null");
                return View("Error");
            }

            _chatroomRepository.Delete(chatRoomDetails);
            return RedirectToAction("Index", "Dashboard");
        }

        // POST: Chatrooms/Pin/1
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Pin(int id)
        {
            var currUser = HttpContext.User.GetUserId();
            var chatroom = await _chatroomRepository.GetByIdAsync(id);
            var pinnedChatrooms = _dashboardRepository.GetAllUserPinnedChatroomsQuery();

            if (await pinnedChatrooms.ContainsAsync(chatroom))
            {
                await _chatroomService.UnpinChatroomAsync(currUser, id);
            }
            else
            {
                await _chatroomService.PinChatroomAsync(currUser, id);
            }

            return Ok();
        }

    }
}
