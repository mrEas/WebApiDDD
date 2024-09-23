using App.Application.Customers.Queries;
using App.Application.Customers.QueryObjects;
using App.Domain.Customers;
using MediatR;

namespace App.Application.Customers.Handlers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto?>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<CustomerDto?> Handle(
            GetCustomerQuery request, 
            CancellationToken cancellationToken)
        {             
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            return new CustomerDto(
                 customer.Id.ToString(),
                 customer.FirstName,
                 customer.LastName,
                 customer.Email.Value,
                 customer.PhoneNumber.Value,
                 customer.Address.Country,
                 customer.Address.Line1,
                 customer.Address.Line2,
                 customer.Address.City,
                 customer.Address.State,
                 customer.Address.ZipCode,
                 true
            );
        }

    }
}
