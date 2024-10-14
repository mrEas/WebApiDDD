using App.Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("users")]
    public class UsersController : ApiControllerBase
    {
        private readonly ISender _sender;
        public UsersController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginQuery query)
        {
            var result = await _sender.Send(query);

            return result.Match(
                user => Ok(user),
                errors => Problem(errors));

        }
          
    }
}
