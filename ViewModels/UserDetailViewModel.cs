using System.ComponentModel.DataAnnotations;

namespace ChatRooms.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}
