using ChatRooms.Helpers;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using ChatRooms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatRooms.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        // GET: Dashboard
        [Authorize]
        public async Task<IActionResult> Index(string sortOrderOwned, string currentFilterOwned, string searchStringOwned, int? pageNumberOwned, string sortOrderPinned, string currentFilterPinned, string searchStringPinned, int? pageNumberPinned)
        {
            ViewData["CurrentSortOwned"] = sortOrderOwned;
            ViewData["NameSortParmOwned"] = String.IsNullOrEmpty(sortOrderOwned) ? "owned_name_desc" : "";

            if (searchStringOwned != null)
            {
                pageNumberOwned = 1;
            }
            else
            {
                searchStringOwned = currentFilterOwned;
            }

            ViewData["CurrentFilterOwned"] = searchStringOwned;

            var userOwnedChatrooms = _dashboardRepository.GetAllUserOwnedChatroomsQuery();

            if (!String.IsNullOrEmpty(searchStringOwned))
            {
                userOwnedChatrooms = userOwnedChatrooms.Where(c => c.Name.ToUpper().Contains(searchStringOwned.ToUpper()));
            }

            switch (sortOrderOwned)
            {
                case "owned_name_desc":
                    userOwnedChatrooms = userOwnedChatrooms.OrderByDescending(c => c.Name);
                    break;
                default:
                    userOwnedChatrooms = userOwnedChatrooms.OrderBy(c => c.Name);
                    break;
            }

            int pageSizeOwned = 2;

            ViewData["CurrentSortPinned"] = sortOrderPinned;
            ViewData["NameSortParmPinned"] = String.IsNullOrEmpty(sortOrderPinned) ? "pinned_name_desc" : "";

            if (searchStringPinned != null)
            {
                pageNumberPinned = 1;
            }
            else
            {
                searchStringPinned = currentFilterPinned;
            }

            ViewData["CurrentFilterPinned"] = searchStringPinned;

            var userPinnedChatrooms = _dashboardRepository.GetAllUserPinnedChatroomsQuery();

            if (!String.IsNullOrEmpty(searchStringPinned))
            {
                userPinnedChatrooms = userPinnedChatrooms.Where(c => c.Name.ToUpper().Contains(searchStringPinned.ToUpper()));
            }

            switch (sortOrderPinned)
            {
                case "pinned_name_desc":
                    userPinnedChatrooms = userPinnedChatrooms.OrderByDescending(c => c.Name);
                    break;
                default:
                    userPinnedChatrooms = userPinnedChatrooms.OrderBy(c => c.Name);
                    break;
            }

            int pageSizePinned = 2;

            var pinnedChatroomsQuery = _dashboardRepository.GetAllUserPinnedChatroomsQuery();

            var dashboardViewModel = new DashboardViewModel()
            {
                OwnedChatrooms = await PaginatedList<Chatroom>.CreateAsync(userOwnedChatrooms.AsNoTracking(), pageNumberOwned ?? 1, pageSizeOwned),
                PinnedChatrooms = await PaginatedList<Chatroom>.CreateAsync(userPinnedChatrooms.AsNoTracking(), pageNumberPinned ?? 1, pageSizePinned),
                PinnedChatroomsQuery = pinnedChatroomsQuery,
            };

            return View(dashboardViewModel);

        }
    }
}
