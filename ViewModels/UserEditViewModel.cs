using System.ComponentModel.DataAnnotations;

namespace ChatRooms.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Username"), Required(ErrorMessage = "Username is required")]
        [MinLength(2, ErrorMessage = "Username must be at least 2 characters")]
        public string UserName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
