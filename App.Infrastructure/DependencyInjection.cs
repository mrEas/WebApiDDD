using App.Domain.Customers;
using App.Domain.Primitives;
using App.Infrastructure.Persistence;
using App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default")));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDataContext>());
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
