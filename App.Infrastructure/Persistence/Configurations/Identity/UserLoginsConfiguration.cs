using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.Configurations.Identity
{
    public class UserLoginsConfiguration : IEntityTypeConfiguration<ApplicationRoleConfiguration>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleConfiguration> builder)
        {
            throw new NotImplementedException();
        }
    }
    }
