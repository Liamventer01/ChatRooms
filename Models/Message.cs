using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatRooms.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

        public int Length { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Chatroom")]
        public int ChatroomId { get; set; }
        public Chatroom? Chatroom { get; set; }
    }
}
