using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations.Identity
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRoleConfiguration>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleConfiguration> builder)
        {
            throw new NotImplementedException();
        }
    }
}
