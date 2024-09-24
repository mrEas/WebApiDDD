using App.Domain.Customers;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Commands
{
    public record DeleteCustomerCommand(Guid CustomerId) : IRequest<ErrorOr<Unit>>;
}
