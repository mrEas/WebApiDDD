using App.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Extensions
{
    public static class MigrationsSeed
    {
        public async static Task ApplyMigrationAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            try
            {
                var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
                await applicationDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDataContext>>();

                logger.LogError(ex, "an error occured during seed migrations");
            }
        }
    }
}
