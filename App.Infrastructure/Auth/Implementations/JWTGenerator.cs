using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Infrastructure.Auth.Abstractions;
using App.Infrastructure.Persistence.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace App.Infrastructure.Auth.Implementations
{ 
    public class JWTGenerator : IJWTGenerator
    { 
        private readonly SymmetricSecurityKey _key;

        public JWTGenerator(IConfiguration config)
        {
            //in secrets
            //dotnet user-secrets init
            //dotnet user-secrets list
            //dotnet user-secrets set "TokenKey" "your-secret-token-key" 
            //dotnet user-secrets remove "TokenKey"
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim> { 
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
