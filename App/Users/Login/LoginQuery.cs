using App.Application.Users.Common;
using ErrorOr;
using MediatR;

namespace App.Application.Users.Login
{
    public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<UserResponse>>;
}
