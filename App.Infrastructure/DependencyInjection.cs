using App.Domain.Customers;
using App.Domain.Primitives;
using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Models;
using App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
namespace App.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDataContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDataContext>());
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddIdentity(configuration);

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSingleton(TimeProvider.System); //для Identity
            services.AddDbContext<ApplicationIdentityContext>(options => options.UseNpgsql(configuration.GetConnectionString("Identity")));

            var builder = services.AddIdentityCore<ApplicationUser>(); //add UserType
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<ApplicationIdentityContext>();

            identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();

            return services;
        }

    }
}
