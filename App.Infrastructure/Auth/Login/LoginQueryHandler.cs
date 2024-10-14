using MediatR;
using Microsoft.AspNetCore.Identity;
using App.Infrastructure.Persistence.Models;
using ErrorOr;
using App.Infrastructure.Auth.Abstractions;
using App.Domain.DomainErrors;
using App.Application.Users.Login;
using App.Application.Users.Common;

namespace App.Infrastructure.Auth.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<UserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJWTGenerator _jwtGenerator;
        public LoginQueryHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJWTGenerator jWTGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jWTGenerator;
        }

        public async Task<ErrorOr<UserResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return UserErrors.UserNotFound;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if(result.Succeeded) {
                return new UserResponse(user.DisplayName, _jwtGenerator.CreateToken(user), user.UserName);
            }
            return UserErrors.UserUnauthorized;

        }
    }
}
