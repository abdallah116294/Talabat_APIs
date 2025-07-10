using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat_APIs.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
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
            services.AddAuthentication();
            return services;
        }
    }
}
