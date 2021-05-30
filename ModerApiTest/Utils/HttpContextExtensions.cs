using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ModerApiTest.Utils
{
    public static class HttpContextExtensions
    {
        public static string GetBearerToken(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var bearer))
            {
                return bearer.ToString().Substring("Bearer ".Length);
            }
            return string.Empty;
        }

        public static string GetCurrentUserId(this HttpContext httpContext)
        {
            return httpContext.User.FindFirstValue(ClaimTypes.Sid);
        }
    }
}
