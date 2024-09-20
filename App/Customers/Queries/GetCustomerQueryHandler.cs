using App.Domain.Customers;
using MediatR;

namespace App.Application.Customers.Queries
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Customer?>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Customer?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            return customer;
        }
         
    }
}
