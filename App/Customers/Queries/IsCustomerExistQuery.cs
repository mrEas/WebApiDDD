using App.Domain.Customers;
using ErrorOr;
using MediatR; 

namespace App.Application.Customers.Queries
{ 
    public record IsCustomerExistQuery(Guid CustomerId) : IRequest<ErrorOr<bool>>;
}
