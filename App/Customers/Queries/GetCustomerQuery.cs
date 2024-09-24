using App.Application.Customers.QueryObjects;
using App.Domain.Customers;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Queries
{
    public record GetCustomerQuery(Guid CustomerId) : IRequest<ErrorOr<CustomerDto>>;
}
