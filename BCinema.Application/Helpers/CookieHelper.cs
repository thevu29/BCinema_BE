using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Helpers;

public class CookieHelper ()
{
    public static void SetCookie(string key, string value, IHttpContextAccessor httpContextAccessor)
    {
        var option = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(1),
        };
        httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, option);
    }
}