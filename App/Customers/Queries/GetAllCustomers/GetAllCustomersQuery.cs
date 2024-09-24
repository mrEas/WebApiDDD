using App.Application.Customers.QueryObjects;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Queries
{
    public record GetAllCustomersQuery : IRequest<ErrorOr<List<CustomerDto>>>;
}
