using App.Infrastructure.Persistence.Models;

namespace App.Infrastructure.Auth.Abstractions
{
    public interface IJWTGenerator
    {
        string CreateToken(ApplicationUser user);
    }
}
