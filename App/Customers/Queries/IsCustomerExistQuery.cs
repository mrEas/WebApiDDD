using App.Domain.Customers;
using MediatR; 

namespace App.Application.Customers.Queries
{ 
    public record IsCustomerExistQuery(CustomerId CustomerId) : IRequest<bool>;
}
