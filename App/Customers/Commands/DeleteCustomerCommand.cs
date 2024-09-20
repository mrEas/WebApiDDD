using App.Domain.Customers;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Commands
{
    public record DeleteCustomerCommand(CustomerId CustomerId) : IRequest<ErrorOr<Unit>>;
}
