using Microsoft.AspNetCore.Builder;
using ModerApiTest.Utils;

namespace ModerApiTest.Middlewares
{
    public static class JWTLogoutSemanticExtensions
    {
        public static IApplicationBuilder UseJWTLogoutSemantic(this IApplicationBuilder builder)
        {
            JWTTokenBlackList.Instance.IsInHttpRequestPipeline = true;
            return builder.UseMiddleware<JWTLogoutSemantic>();
        }
    }
}
