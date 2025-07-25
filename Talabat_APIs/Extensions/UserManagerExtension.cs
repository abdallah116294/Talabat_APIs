using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat_APIs.Extensions
{
    public static  class UserManagerExtension
    {
        //Extension methods to find user with its address 
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser>userManager,ClaimsPrincipal User) 
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            //LINQ for including data
            var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
