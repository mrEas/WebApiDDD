using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Persistence.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
