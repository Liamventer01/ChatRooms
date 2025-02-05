using ChatRooms.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatRooms.Data
{
    public class DbInitializer
    {
        public static void CreateDatabase(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ChatroomContext>();

                //context?.Database.EnsureDeleted();
                context?.Database.EnsureCreated();
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserName = "etienneT";

                var adminUser = await userManager.FindByNameAsync(adminUserName);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        Id = "bc100ece-cdd0-481a-b0a0-a8ec05dca602",
                        UserName = "etienneT",
                        ProfileImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713456/u0mfbpyi0nnltpj4gg16.png"
                    };
                    await userManager.CreateAsync(newAdminUser, "Password@64");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserName = "liamV";

                var appUser = await userManager.FindByNameAsync(appUserName);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        Id = "bbd74782-525a-4c59-9700-5a0b728bf0c6",
                        UserName = "liamV",
                        ProfileImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692714244/pvjnhty0f2sywne9taaz.jpg"
                    };
                    await userManager.CreateAsync(newAppUser, "Password@64");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                string appUserName2 = "test";

                var appUser2 = await userManager.FindByNameAsync(appUserName2);
                if (appUser2 == null)
                {
                    var newAppUser2 = new User()
                    {
                        Id = "a6e31363-fc72-4f7e-9238-7f6cada1e68c",
                        UserName = "test",
                        ProfileImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692714457/gjc1mwvb17ghky7dvi1u.png"
                    };
                    await userManager.CreateAsync(newAppUser2, "Password@64");
                    await userManager.AddToRoleAsync(newAppUser2, UserRoles.User);
                }
            }
        }

        public static void Initialize(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ChatroomContext>();

                context?.Database.EnsureCreated();
                // Chatrooms
                if (!context.Chatrooms.Any())
                {
                    context.Chatrooms.AddRange(new List<Chatroom>()
                    {
                        new Chatroom()
                        {
                            Name = "Global Chat X",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692533555/glbz9vizpsqt9hwateb6.webp",
                            Description = "This is the town square, all are welcome, free speech",
                            MsgLengthLimit = 0,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"

                         },
                        new Chatroom()
                        {
                            Name = "Fortnite",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692532935/j1on5owlohilcs53phad.jpg",
                            Description = "We love fortnite Poggers",
                            MsgLengthLimit = 280,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"

                         },
                       new Chatroom()
                        {
                            Name = "Baldurs Gate 3",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692532380/ka3n1304vzib1u8ctsvd.webp",
                            Description = "Dungeons and dragons game with roll playing and dice rolling",
                            MsgLengthLimit = 269,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },
                        new Chatroom()
                        {
                            Name = "CSGO",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713521/y49mo720z4uwmjilpsdh.jpg",
                            Description = "The best shooter ever",
                            MsgLengthLimit = 120,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },
                         new Chatroom()
                        {
                            Name = "Minecraft",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713542/i6tnfgzrxuycdd1nvdpn.webp",
                            Description = "This game never gets old",
                            MsgLengthLimit = 240,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },
                           new Chatroom()
                        {
                            Name = "Terraria",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713574/ufqxs7ruwuqh5dlft5zq.webp",
                            Description = "2D Action-packed Awesomeness",
                            MsgLengthLimit = 200,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },
                           new Chatroom()
                        {
                            Name = "CS2",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713593/r85yoxynl5wokv6cgjab.jpg",
                            Description = "When will this game come out",
                            MsgLengthLimit = 200,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },

                           new Chatroom()
                        {
                            Name = "Dragon Ball",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713622/guonhiywx2147h18wr7d.png",
                            Description = "Best anime no questions asked",
                            MsgLengthLimit = 200,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },

                           new Chatroom()
                        {
                            Name = "Age of empires 4",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692713653/jeo3i4lxunol5xhjy4z1.webp",
                            Description = "Realtime Strategy game, much better than Civ 6",
                            MsgLengthLimit = 0,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },
                            new Chatroom()
                        {
                            Name = "🎉 EmojiPalooza 🌈",
                            ChatroomImageUrl = "http://res.cloudinary.com/dzjsiibch/image/upload/v1692863067/t23oikpvy2gxuetddw20.jpg",
                            Description = "👋😄🤖🔤🔡🔠📝🔣🆗",
                            MsgLengthLimit = 2,
                            OwnerId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602"
                         },

                    });
                    context.SaveChanges();
                }
                // Messages
                if (!context.Messages.Any())
                {
                    context.Messages.AddRange(new List<Message>()
                    {
                        new Message()
                        {
                            Content = "<p>Hello everyone, how is it going!</p>",
                            Length = "Hello everyone, how is it going!".Length,
                            TimeStamp = DateTime.Parse("2023/08/01"),
                            UserId = "bbd74782-525a-4c59-9700-5a0b728bf0c6",
                            ChatroomId=1,
                        },
                         new Message()
                        {
                            Content = "<p>It not growing so great, my website is a disaster!</p>",
                            Length = "It not growing so great, my website is a disaster!".Length,
                            TimeStamp = DateTime.Parse("2023/08/02"),
                            UserId = "bc100ece-cdd0-481a-b0a0-a8ec05dca602",
                            ChatroomId=1,
                        },
                           new Message()
                        {
                            Content = "<p>Sucks to suck hey</p>",
                            Length = "Sucks to suck hey".Length,
                            TimeStamp = DateTime.Parse("2023/08/03"),
                            UserId = "bbd74782-525a-4c59-9700-5a0b728bf0c6",
                            ChatroomId=1,
                        },
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
