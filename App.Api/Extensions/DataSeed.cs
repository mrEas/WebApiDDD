using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Models;
using App.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Identity;

namespace App.Api.Extensions
{
    public static class DataSeed
    { 
        //https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorWebAssemblyStandaloneWithIdentity/Backend/SeedData.cs
        public async static Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<Program>>(); 

            try
            {
                var context = services.GetRequiredService<ApplicationDataContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await SeedCustomersAsync(context, logger);
                await SeedIdentityAsync(userManager, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during data seeding.");
            }

        }
        private static async Task SeedCustomersAsync(ApplicationDataContext context, ILogger logger)
        {
            logger.LogInformation("Seeding customers...");
            await CustomersSeed.SeedDataAsync(context);
            logger.LogInformation("Customers seeded successfully.");
        }

        private static async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            logger.LogInformation("Seeding identity data...");
            await IdentitySeed.SeedDataAsync(userManager);
            logger.LogInformation("Identity data seeded successfully.");
        }


    }
}
