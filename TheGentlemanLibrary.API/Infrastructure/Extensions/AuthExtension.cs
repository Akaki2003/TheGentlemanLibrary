using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TheGentlemanLibrary.Application.Models.Users.JWT;

namespace TheGentlemanLibrary.API.Infrastructure.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddMyAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetSection(nameof(JWTConfiguration)).GetSection(nameof(JWTConfiguration.Secret)).Value;
            var keybytes = Encoding.ASCII.GetBytes(key);
            services.AddAuthentication(
                    x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
                .AddJwtBearer(x =>
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(keybytes),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "localhost",
                    ValidAudience = "localhost",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                })
                ;


            services.AddAuthorizationBuilder()
                .AddPolicy("MyPolicy", policy =>
                {
                    policy.Requirements.Add(new OperationAuthorizationRequirement());
                })
                 .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build())
                 ;
            return services;
        }
    }
}
