using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Servcies;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat_APIs.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<AppUser, IdentityRole>(
                Options =>
                {
                    Options.Password.RequireDigit = false;
                    Options.Password.RequiredLength = 6;
                    Options.Password.RequireNonAlphanumeric = false;
                    Options.Password.RequireUppercase = false;
                    Options.Password.RequireLowercase = false;
                }
                )
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                Options =>
                {
                    Options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true, // Validate the issuer of the token
                        ValidIssuer = config["JWT:ValidIssuer"], // The valid issuer of the token
                        ValidateAudience = true, // Validate the audience of the token
                        ValidAudience = config["JWT:ValidAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]))
                    };
                }
                );
            services.AddScoped<ITokenService, TokenService>(); // Register the token service for JWT generation
            return services;
        }
    }
}
