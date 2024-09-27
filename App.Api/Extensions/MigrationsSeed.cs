using App.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Extensions
{
    public static class MigrationsSeed
    {
        public async static Task ApplyMigrationAsync(this WebApplication app)
        { 
            await ApplyMigrationForContext<ApplicationDataContext>(app);
            await ApplyMigrationForContext<ApplicationIdentityContext>(app);
            
            if (app.Environment.IsDevelopment())
            {
                await app.SeedDataAsync();
            }

        }

        private static async Task ApplyMigrationForContext<TContext>(WebApplication app) where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();

            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
                logger.LogError(ex, "An error occurred during migrations for {DbContextName}", typeof(TContext).Name);
            }
        }
    }
}
