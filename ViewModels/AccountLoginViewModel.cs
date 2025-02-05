using System.ComponentModel.DataAnnotations;

namespace ChatRooms.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
