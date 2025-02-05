using ChatRooms.Interfaces;
using ChatRooms.Models;
using ChatRooms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChatRooms.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatroomRepository _chatroomRepository;
        private readonly IPhotoService _photoService;
        private readonly IMessageRepository _messageRepository;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserRepository userRepository, IChatroomRepository chatroomRepository, IMessageRepository messageRepository, IPhotoService photoService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _chatroomRepository = chatroomRepository;
            _messageRepository = messageRepository;
            _photoService = photoService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: User
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currUser = HttpContext.User.GetUserId();
            var user = await _userRepository.GetUserByIdAsync(currUser);

            int userChatroomsCount = _userRepository.CountChatroomsByUserId(currUser);
            int userPinnedChatroomsCount = _userRepository.CountPinnedChatroomsByUserId(currUser);
            int userMessagesCount = _userRepository.CountMessagesByUserId(currUser);

            var userVM = new UserIndexViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
                OwnedChatrooms = userChatroomsCount,
                PinnedChatrooms = userPinnedChatroomsCount,
                UserMessages = userMessagesCount,
            };

            return View(userVM);
        }

        // GET: User/Edit/1
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                // Logging runtime exception
                Log.Error("User is null");
                return View("Error");
            }
            var userVM = new UserEditViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(userVM);
        }

        // POST: User/Edit/1
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, UserEditViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("Edit", userVM);
            }

            if (userVM.Image == null)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    // Logging runtime exception
                    Log.Error("User is null");
                    return View("Error");
                }

                try
                {
                    user.UserName = userVM.UserName;
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    // Logging runtime exception
                    Log.Error("Unhandled exception occurred", ex);
                    return View("Error");
                }

                return RedirectToAction("Index");
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    // Logging runtime exception
                    Log.Error("User is null");
                    return View("Error");
                }

                var photoResult = await _photoService.AddProfilePictureAsync(userVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(userVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                try
                {
                    user.UserName = userVM.UserName;
                    user.ProfileImageUrl = photoResult.Url.ToString();

                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    // Logging runtime exception
                    Log.Error("Unhandled exception occurred", ex);
                    return View("Error");
                }

                return RedirectToAction("Index");
            }
        }


        // GET: User/Delete/1
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Logging runtime exception
                Log.Error("User is null");
                return View("Error");
            }

            var userVM = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(userVM);
        }

        // POST: User/Delete/1
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Logging runtime exception
                Log.Error("User is null");
                return View("Error");
            }

            try
            {
                _messageRepository.DeleteMessagesByUserId(id);
                _chatroomRepository.DeletePinnedChatroomsByUserId(id);
                _chatroomRepository.DeleteChatroomsByUserId(id);

                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                // Logging runtime exception
                Log.Error("Unhandled exception occurred", ex);
                return RedirectToAction("Error", "Home");
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
