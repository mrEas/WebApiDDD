using App.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager)
        {
            if (!await userManager.Users.AnyAsync())
            {
                var users = new List<ApplicationUser>()
                {
                    new ApplicationUser
                    {
                        DisplayName= "Ivanov", 
                        UserName = "IvanIvanov",
                        FirstName = "Ivanov",
                        LastName= "Ivanov",
                        Email = "IvanIvanov@gmail.com"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "PetrIvanov",
                        UserName = "PetrIvanov",
                        FirstName = "Petr",
                        LastName = "Ivanov",
                        Email = "PetrIvanov@gmail.com"
                    },
                };

                foreach (var user in users)
                {
                   await userManager.CreateAsync(user, "Qwerty123!");
                }
            }
        }
    }
}
