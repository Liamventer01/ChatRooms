using ChatRooms.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatRooms.Data
{
    public class ChatroomContext : IdentityDbContext<User>
    {
        public ChatroomContext(DbContextOptions<ChatroomContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserPinnedChatroom>()
                .HasKey(upc => new { upc.UserId, upc.ChatroomId });

            modelBuilder.Entity<UserPinnedChatroom>()
                .HasOne(upc => upc.User)
                .WithMany(u => u.PinnedChatrooms)
                .HasForeignKey(upc => upc.UserId);

            modelBuilder.Entity<UserPinnedChatroom>()
                .HasOne(upc => upc.Chatroom)
                .WithMany()  // No navigation property on Chatroom for pinned chatrooms
                .HasForeignKey(upc => upc.ChatroomId);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<UserPinnedChatroom> UserPinnedChatrooms { get; set; }
    }
}
