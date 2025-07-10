using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentitiyDbContextSeed
    {
        public static async Task SeedUser(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Abdallah Mohamed",
                    Email = "abdallhamohamed116@gmail.com",
                    UserName = "abdallhamohamed116",
                    PhoneNumber = "01144585024"
                };

                await userManager.CreateAsync(User, "Pass@123");
            }
           
        } 
    }
}
