using System.Security.Claims;

namespace ChatRooms
{
    public static class ClaimsPrincipalExtensions
    {
        // Shorter way of getting the User Id
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
