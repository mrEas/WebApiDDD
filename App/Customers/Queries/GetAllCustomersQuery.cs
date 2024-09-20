using App.Application.Customers.QueryObjects;
using MediatR;

namespace App.Application.Customers.Queries
{
    public record GetAllCustomersQuery : IRequest<List<CustomerDto>>;

}
