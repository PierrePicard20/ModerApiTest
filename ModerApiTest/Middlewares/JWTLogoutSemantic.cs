using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ModerApiTest.Utils;

namespace ModerApiTest.Middlewares
{
    public class JWTLogoutSemantic
    {
        private readonly RequestDelegate _next;

        public JWTLogoutSemantic(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Check that the JWT token is not in the black list
            bool blackListed = false;
            var token = httpContext.GetBearerToken();
            if (token != null)
            {
                blackListed = JWTTokenBlackList.Instance.IsBlackListed(token);
            }

            if (blackListed)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                // chain to the next midleware
                await _next(httpContext);
            }
        }
    }
}
