using App.Infrastructure.Persistence.Configurations.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Persistence
{
    //IdentityDbContext<TUser,TRole,TKey,IdentityUserClaim<TKey>,IdentityUserRole<TKey>,IdentityUserLogin<TKey>,IdentityRoleClaim<TKey>,IdentityUserToken<TKey>>
    // для того чтобы использовать нашу связку многие ко многим меэжу ролью и юзером
    public class ApplicationIdentityContext : IdentityDbContext<ApplicationUserConfiguration,
            ApplicationRoleConfiguration, string,
            IdentityUserClaim<string>,
            ApplicationUserRoleConfiguration,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>>
    {

        private const string SCHEME = "Identity";
        private readonly IConfiguration _configuration;

        public ApplicationIdentityContext(DbContextOptions<ApplicationDataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public ApplicationIdentityContext(){ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=app;User Id=botadmin;Password=12345678;");
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
