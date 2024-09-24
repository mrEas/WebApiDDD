using App.Application.Customers.QueryObjects;
using App.Domain.Customers;
using App.Domain.DomainErrors;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Queries
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, ErrorOr<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<ErrorOr<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

            if (customer == null)
            {
                return CustomerErrors.CustomerNotFound;
            }

            return new CustomerDto(
                 request.CustomerId,
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
