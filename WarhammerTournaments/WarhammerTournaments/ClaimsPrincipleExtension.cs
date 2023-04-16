using System.Security.Claims;

namespace WarhammerTournaments;

public static class ClaimsPrincipleExtension
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}