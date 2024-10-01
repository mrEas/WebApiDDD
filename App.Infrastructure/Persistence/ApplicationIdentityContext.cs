using App.Infrastructure.Persistence.Configurations.Customers;
using App.Infrastructure.Persistence.Configurations.Identity;
using App.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 

namespace App.Infrastructure.Persistence
{
    //IdentityDbContext<TUser,TRole,TKey,IdentityUserClaim<TKey>,IdentityUserRole<TKey>,IdentityUserLogin<TKey>,IdentityRoleClaim<TKey>,IdentityUserToken<TKey>>
    // для того чтобы использовать нашу связку многие ко многим между ролью и юзером
    public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser,
        ApplicationRole,string,
        IdentityUserClaim<string>,
        ApplicationUserRole,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
        private const string SCHEME = "Identity";
        private readonly IConfiguration _configuration;

        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        //dotnet ef migrations add Initial -c ApplicationIdentityContext -s App.Api -p App.Infrastructure -o Persistence/Migrations/IdentityMigrations
        //dotnet ef database update -c ApplicationIdentityContext -s App.Api -p App.Infrastructure 
        //dotnet ef database drop -c ApplicationIdentityContext -s App.Api -p App.Infrastructure
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(SCHEME));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.HasDefaultSchema(SCHEME);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            builder.ApplyConfiguration(new UserClaimsConfiguration());
            builder.ApplyConfiguration(new RoleClaimsConfiguration());
            builder.ApplyConfiguration(new UserLoginsConfiguration());
            builder.ApplyConfiguration(new UserTokensConfiguration());
        }

    }
}
