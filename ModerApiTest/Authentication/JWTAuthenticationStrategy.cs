using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using ModerApiTest.Models;
using ModerApiTest.Middlewares;
using MongoDB.Bson;
using ModerApiTest.Utils;

namespace ModerApiTest.Authentication
{
    public static class JWTAuthenticationStrategy
    {
        public static void ConfigureAuthenticationService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Issuer"],
                };
            });
        }

        public static void UsePostAuthentication(IApplicationBuilder app)
        {
            // checks the token is not black listed (after logout)
            app.UseJWTLogoutSemantic();
        }

        public static (string,DateTime) GenerateToken(LoginModel loginModel, ObjectId userId, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(configuration["JWT:KEY"]);
            var expire = DateTime.UtcNow.AddHours(1);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, loginModel.email)
                                                                                , new Claim(ClaimTypes.Sid, userId.ToString())}),
                Expires = expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Issuer"]
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            JWTTokenBlackList.Instance.AddToken(token, expire);
            return (token,expire);
        }

    }
}
