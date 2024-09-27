using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Persistence.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
