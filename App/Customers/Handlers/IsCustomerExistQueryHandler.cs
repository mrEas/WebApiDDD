using App.Application.Customers.Queries;
using App.Domain.Customers;
using App.Domain.DomainErrors;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Handlers
{
    public class IsCustomerExistQueryHandler : IRequestHandler<IsCustomerExistQuery, ErrorOr<bool>>
    {
        private readonly ICustomerRepository _customerRepository;
        public IsCustomerExistQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ErrorOr<bool>> Handle(IsCustomerExistQuery request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.IsExistAsync(new CustomerId(request.CustomerId));
            if (!result)
            {
                return CustomerErrors.CustomerNotFound;
            }
            
            return result;
        } 
        
    }
}
