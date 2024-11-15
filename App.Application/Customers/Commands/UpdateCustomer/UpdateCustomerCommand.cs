using MediatR;
using ErrorOr;

namespace App.Application.Customers.Commands
{
    public record UpdateCustomerCommand(
        Guid CustomerId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Country,
        string Line1,
        string Line2,
        string City,
        string State,
        string ZipCode) : IRequest<ErrorOr<Unit>>;
}
